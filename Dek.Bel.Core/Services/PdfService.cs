﻿using Dek.Cls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Annot;
using Dek.Bel.Core.Models;
using iText.Kernel.Pdf.Filespec;
using iText.Kernel.Pdf.Action;
using iText.Layout.Borders;
//using iText.Kernel.Pdf.Colorspace;
using System.Threading;
using Dek.Bel.Core.GUI;
using Dek.Bel.Core.Cls;
using Dek.Bel.Core.Helpers;

namespace Dek.Bel.Core.Services
{
    [Export(typeof(IPdfService))]
    public class PdfService : IPdfService
    {

        [Import] public IUserSettingsService m_UserSettingsService { get; set; }
        [Import] public ICategoryService m_CategoryService { get; set; }
        [Import] public IMessageboxService m_MessageboxService { get; set; }
        [Import] public TempFileService m_TempFileService { get; set; }
        [Import] public IStorageHelperService m_StorageHelperService { get; set; }

        public void RecreateTheWholeThing(ModelsForViewing vm, VolumeService volumeService)
        {
            string origFileName = vm.CurrentStorage.FilePath;
            string storageFilePath = System.IO.Path.Combine(m_UserSettingsService.StorageFolder, vm.CurrentStorage.StorageName);
            string newFileTmp = m_TempFileService.GetNewTmpFileName(vm.CurrentStorage.StorageName);
            //File.Copy(storageFilePath, tmpFileName, true);
            (int x, int y) offset = (volumeService.CurrentVolume.OffsetX, volumeService.CurrentVolume.OffsetY);

            PdfDocument pdfDoc = new PdfDocument(new PdfReader(origFileName), new PdfWriter(newFileTmp));

            foreach (Citation cit in volumeService.Citations)
            {
                // Yeah, somtimes illegal citations end up in the database.
                if (cit.SelectionRects.Trim().Length < 5)
                    continue;

                var cat = m_CategoryService.GetMainCategory(cit.Id);
                var citCat = m_CategoryService.GetMainCitationCategory(cit.Id);

                if (cat == null)
                    citCat.CategoryId = Id.Empty;

                List<(int page, int[] rects)> pageRects = ArrayStuff.ConvertStringToPagesAndArrays(cit.SelectionRects);
                int firstPageNo = GetFirstPageFromPageRects(pageRects);

                Color color_highLight, color_underLine, color_margin = new DeviceRgb(n(255), n(255), n(255)); ;

                if (pageRects.Any())
                {
                    System.Drawing.Color ch, cu, cm;
                    var colors = ColorStuff.ConvertStringToColors(cit.CitationColors);
                    ch = (colors.Length >= 1)
                        ? colors[0]
                        : m_UserSettingsService.PdfHighLightColor;
                    cu = (colors.Length >= 2)
                        ? colors[1]
                        : m_UserSettingsService.PdfUnderlineColor;
                    cm = (colors.Length >= 3)
                        ? colors[2]
                        : m_UserSettingsService.PdfMarginBoxColor;

                    color_highLight = new DeviceRgb(n(ch.R), n(ch.G), n(ch.B));
                    color_underLine = new DeviceRgb(n(cu.R), n(cu.G), n(cu.B));
                    color_margin = new DeviceRgb(n(cm.R), n(cm.G), n(cm.B));

                    foreach ((int page, int[] rects) pageRect in pageRects)
                    {
                        if (pageRect.rects.Count() < 4) // If no rects, for some reason...
                            continue;

                        //int currentPage = pageRects.First().page;
                        int currentPage = pageRect.page;
                        int currentRect = 0;
                        bool notDone = true;
                        do
                        {
                            PdfCanvas canvas1 = new PdfCanvas(
                                pdfDoc.GetPage(currentPage).NewContentStreamBefore(),
                                pdfDoc.GetPage(currentPage).GetResources(), pdfDoc);
                            PdfCanvas canvas2 = new PdfCanvas(
                                pdfDoc.GetPage(currentPage).NewContentStreamAfter(),
                                pdfDoc.GetPage(currentPage).GetResources(), pdfDoc);

                            Rectangle pageSize = pdfDoc.GetPage(firstPageNo).GetPageSize();
                            int pageHeight = (int)pageSize.GetHeight();
                            int pageWidth = (int)pageSize.GetWidth();

                            //canvas.SaveState();

                            canvas1.SetFillColor(color_highLight);
                            canvas2.SetFillColor(color_underLine);

                            do //(int i = 0; i < rects.Length; i += 4)
                            {
                                if (currentRect >= pageRect.rects.Length) // Sometime this is necessary, page rects are empty or broken.
                                    break;

                                int x = pageRect.rects[currentRect] + offset.x;
                                int y = pageHeight - pageRect.rects[currentRect + 1] - pageRect.rects[currentRect + 3] + offset.y;
                                // Below text highlight
                                canvas1.Rectangle(x, y, pageRect.rects[currentRect + 2], pageRect.rects[currentRect + 3]);
                                canvas1.Fill();
                                // Above text underline
                                canvas2.Rectangle(x, y, pageRect.rects[currentRect + 2], 1);
                                canvas2.Fill();

                                if (currentRect + 5 > pageRect.rects.Length)
                                {
                                    notDone = false;
                                    break;
                                }

                                // Detect page break
                                int lastY = pageHeight - pageRect.rects[currentRect + 1] - pageRect.rects[currentRect + 3];
                                currentRect += 4;
                                int newY = pageHeight - pageRect.rects[currentRect + 1] - pageRect.rects[currentRect + 3];
                                if (lastY < newY)
                                {
                                    currentPage++;
                                    break;
                                }

                            } while (true);
                        } while (notDone);
                    }

                    // Add annotation
                    AddMarginBox(pdfDoc, null, firstPageNo, cit.Id.ToString(), cat.Code + $" [{citCat.Weight}]", cit.SelectionRects,
                        color_margin,
                        cit.MarginBoxSettings,
                        offset);
                }// if pagerects.any

            }
            pdfDoc.Close(); // newFileTmp
            m_StorageHelperService.GetNextStorageFileName(vm.CurrentStorage);
            string newStoragePath = System.IO.Path.Combine(m_UserSettingsService.StorageFolder, vm.CurrentStorage.StorageName);

            int retries = 10;
            do
            {
                try
                {
                    //File.Delete(storageFilePath);
                    File.Copy(newFileTmp, newStoragePath);
                    retries = 0;
                }
                catch
                {
                    retries--;
                    Thread.Sleep(500);
                }
            } while (retries > 0);

            File.Delete(newFileTmp);
            m_StorageHelperService.DeleteOldStorageFiles(newStoragePath);
        }

        public void AddMarginBox(
            PdfDocument pdfDoc,
            PdfAnnotation existingAnnot,
            int physicalPage,
            string id,
            string text,
            string pageRectsString,
            Color marginColor,
            string pdfMarginBoxSettings,
            (int x, int y) offset
            )
        {
            PdfMarginBoxSettings settings = new PdfMarginBoxSettings(pdfMarginBoxSettings);

            switch (settings.DisplayMode)
            {
                case Constants.MarginBoxVisualMode.Compact:
                case Constants.MarginBoxVisualMode.Vertical:
                    AddMarginBox(
                        pdfDoc,
                        existingAnnot,
                        physicalPage,
                        id,
                        text,
                        pageRectsString,
                        marginColor,
                        settings.Width,
                        settings.Height,
                        settings.Margin,
                        settings.BorderThickness,
                        settings.Font,
                        settings.FontSize,
                        settings.RightMargin,
                        settings.DisplayMode,
                        offset);
                    break;
                case Constants.MarginBoxVisualMode.Hidden:
                    break;
                case Constants.MarginBoxVisualMode.Minimal:
                    AddMarginBoxMinimal(
                        pdfDoc,
                        existingAnnot,
                        physicalPage,
                        id,
                        text,
                        pageRectsString,
                        marginColor,
                        settings.Height,
                        settings.Margin,
                        settings.BorderThickness,
                        settings.RightMargin,
                        offset);
                    break;
                default: // Normal
                    AddMarginBox(
                        pdfDoc,
                        existingAnnot,
                        physicalPage,
                        id,
                        text,
                        pageRectsString,
                        marginColor,
                        settings.Width,
                        settings.Height,
                        settings.Margin,
                        settings.BorderThickness,
                        settings.Font,
                        settings.FontSize,
                        settings.RightMargin,
                        Constants.MarginBoxVisualMode.Normal,
                        offset);
                    break;
            }

        }

        public void AddMarginBox(
            PdfDocument pdfDoc,
            PdfAnnotation existingAnnot,
            int physicalPage,
            string id,
            string text,
            string pageRectsString,
            Color marginColor,
            int width,
            int height,
            int margin,
            float borderThickness,
            string font,
            float fontSize,
            bool rightMargin,
            string visualMode,
            (int x, int y) offset)
        {
            bool annotExists = existingAnnot != null;

            // Compact = minimize width - by multiline!
            int noOfLines = 1;
            if (visualMode == Constants.MarginBoxVisualMode.Compact)
            {
                text = text.Replace("      ", " ").Replace("     ", " ").Replace("    ", " ").Replace("   ", " ").Replace("  ", " ");
                noOfLines = text.Split(' ').Count();
                text = text.Replace(" ", Environment.NewLine);
            }

            int yCoord = GetMinMaxYCoordFromFirstPage(pageRectsString).min;

            Rectangle pageSize = pdfDoc.GetPage(physicalPage).GetPageSize();
            int pageHeight = (int)pageSize.GetHeight();
            int pageWidth = (int)pageSize.GetWidth();

            int boxWidth = width;
            int boxHeight = height * noOfLines; // Vague attempt at getting position correct when using compact multiline
            int boxLeft = rightMargin ?
                pageWidth - (margin + boxWidth)
                : margin;
            int boxBottom = pageHeight - yCoord - boxHeight;

            if (visualMode == Constants.MarginBoxVisualMode.Vertical)
            {
                if (rightMargin)
                {
                    boxLeft = pageWidth - height - margin;
                    boxBottom += height;
                }
                else
                {
                    boxLeft += height;
                    boxBottom += (height - width);
                }

            }

            boxLeft += offset.x;
            boxBottom += offset.y;

            // Page number labels
            pdfDoc.GetFirstPage().SetPageLabel(PageLabelNumberingStyle.DECIMAL_ARABIC_NUMERALS, null);

            // Action
            PdfFileSpec spec = PdfFileSpec.CreateExternalFileSpec(pdfDoc, $"belpdf:{id}");
            PdfAction action = PdfAction.CreateLaunch(spec);

            PdfFont pdfFont = PdfFontFactory.CreateFont(font);

            Link link = new Link(text, action);
            link.GetLinkAnnotation().SetName(new PdfString($"belpdf:{id}"));
            var paragraph1 = new iText.Layout.Element.Paragraph()
                .Add(link)
                .SetStrokeColor(ColorConstants.BLACK)
                .SetFontColor(ColorConstants.BLACK)
                .SetFixedPosition(boxLeft, boxBottom, boxWidth) // NB that height is variable
                .SetFont(pdfFont)
                .SetFontSize(fontSize)
                .SetPageNumber(physicalPage);

            var mc = marginColor.GetColorValue();
            if ((mc[0] != 255) && (mc[1] != 255) && (mc[2] != 255))
                paragraph1.SetBackgroundColor(marginColor);

            if (borderThickness > 0f)
                paragraph1.SetBorder(new SolidBorder(borderThickness));

            if (visualMode == Constants.MarginBoxVisualMode.Vertical)
                paragraph1.SetRotationAngle((rightMargin ? -Math.PI / 2 : Math.PI / 2));

            Document doc = new Document(pdfDoc);
            doc.Add(paragraph1);
        }

        public void AddMarginBoxMinimal(
            PdfDocument pdfDoc,
            PdfAnnotation existingAnnot,
            int physicalPage,
            string id,
            string text,
            string pageRectsString,
            Color marginColor,
            int height,
            int margin,
            float borderThickness,
            bool rightMargin,
            (int x, int y) offset)
        {
            bool annotExists = existingAnnot != null;

            int yCoord = GetMinMaxYCoordFromFirstPage(pageRectsString).min;

            Rectangle pageSize = pdfDoc.GetPage(physicalPage).GetPageSize();
            int pageHeight = (int)pageSize.GetHeight();
            int pageWidth = (int)pageSize.GetWidth();

            int boxHeight = height; // Vague attempt at getting position correct when using compact multiline
            int boxLeft = rightMargin ?
                pageWidth - (margin + boxHeight)
                : margin;
            int boxBottom = pageHeight - yCoord - boxHeight;

            boxLeft += offset.x;
            boxBottom += offset.y;

            // Page number labels
            pdfDoc.GetFirstPage().SetPageLabel(PageLabelNumberingStyle.DECIMAL_ARABIC_NUMERALS, null);

            // Action
            PdfFileSpec spec = PdfFileSpec.CreateExternalFileSpec(pdfDoc, $"belpdf:{id}");
            PdfAction action = PdfAction.CreateLaunch(spec);

            PdfFont pdfFont = PdfFontFactory.CreateFont(Constants.PdfFont.TIMES_ROMAN);

            Link link = new Link(text.Substring(0, 1), action);
            link.GetLinkAnnotation().SetName(new PdfString($"belpdf:{id}"));
            var paragraph1 = new iText.Layout.Element.Paragraph()
                .Add(link)
                .SetStrokeColor(ColorConstants.BLACK)
                .SetFontColor(ColorConstants.BLACK)
                .SetFixedPosition(boxLeft, boxBottom, boxHeight) // NB that height is variable
                .SetFont(pdfFont)
                .SetFontSize(8)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetPageNumber(physicalPage);

            var mc = marginColor.GetColorValue();
            if ((mc[0] != 255) && (mc[1] != 255) && (mc[2] != 255))
                paragraph1.SetBackgroundColor(marginColor);

            if (borderThickness > 0f)
                paragraph1.SetBorder(new SolidBorder(borderThickness));

            Document doc = new Document(pdfDoc);
            doc.Add(paragraph1);
        }

        private bool RemoveAnnotation(string storageFileName, int physicalPage, string citationId)
        {
            string storageFilePath = System.IO.Path.Combine(m_UserSettingsService.StorageFolder, storageFileName);
            string tmpFileName = m_TempFileService.GetNewTmpFileName(storageFileName);

            PdfDocument pdfDoc = new PdfDocument(new PdfReader(storageFilePath), new PdfWriter(tmpFileName));
            PdfPage pdfPage = pdfDoc.GetPage(physicalPage);
            List<PdfAnnotation> annots = pdfPage.GetAnnotations().ToList();

            if (annots == null || annots.Count == 0)
            {
                pdfDoc.Close();
                return false;
            }
            else
            {
                foreach (PdfAnnotation annot in annots)
                {
                    if (annot is PdfLinkAnnotation linkAnnotation)
                    {
                        if (linkAnnotation.GetName()?.ToString() == $"belpdf:{citationId}")
                            pdfPage.RemoveAnnotation(annot);
                    }
                }
            }

            pdfDoc.Close();

            File.Delete(storageFilePath);
            File.Move(tmpFileName, storageFilePath);
            File.Delete(tmpFileName);

            return true;
        }

        /// <summary>
        /// Find topmost y coord from first page mentioned in pageRectString.
        /// </summary>
        public (int min, int max) GetMinMaxYCoordFromFirstPage(string pageRectsString)
        {
            List<(int page, int[] rects)> pageRects = ArrayStuff.ConvertStringToPagesAndArrays(pageRectsString);
            if (!pageRects.Any())
                return (0, 0);

            int firstPage = pageRects.OrderBy(x => x.page).First().page;
            // All rects from first page in one big happy array
            int[] firstPageRects = pageRects
                .Where(x => x.page == firstPage)
                .SelectMany(s => s.rects)
                .ToArray();
            // Every fourth nbr starting with [1] is an y coord.
            int len = firstPageRects.Length;
            int max = int.MinValue;
            int min = int.MaxValue;
            for (int i = 0; i < len; i += 4)
            {
                int y = firstPageRects[i + 1];
                if (y > max)
                    max = y;

                if (y < min)
                    min = y;
            }

            return (min, max);
        }

        private int GetFirstPageFromPageRects(List<(int page, int[] rects)> pageRects)
        {
            int minPageNo = int.MaxValue;
            foreach (int pageNo in pageRects.Select(x => x.page))
                if (pageNo < minPageNo)
                    minPageNo = pageNo;

            return minPageNo;
        }


        /// <summary>
        /// Returns outer containing bounds of rect.
        /// Textrange used as x-coords
        /// </summary>
        /// <param name="rects"></param>
        /// <returns></returns>
        private DekRange GetLeftAndRightBounds(int[] rects)
        {
            int min = int.MaxValue;
            int max = 0;
            int count = rects.Count() / 4;
            for (int i = 0; i < rects.Count() / 4; i++)
            {
                if (rects[i] < min)
                    min = rects[i];
                if (rects[i] + rects[i + 2] > max)
                    max = rects[i] + rects[i + 2];
            }

            return new DekRange(min, max);
        }

        /// <summary>
        /// Normalize byte to float
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private float n(byte b)
        {
            return (b == 0) ? 0f : (float)b / 255;
        }

        /*
         
        //The following code snippet copies and modifies the text content of FreeText annotations from 1 PDF (i.e. annots) 
        // and saves the modified annotations into a new PDF. 
        // A good chunk of the code is similar to the answer of this post (https://stackoverflow.com/questions/37014984/how-to-read-text-of-appearance-stream?noredirect=1&lq=1) but was updated for iText7.

         foreach (var anno in annots)
{
    var a = anno.GetPdfObject().CopyTo(masterPdfDoc);
    PdfAnnotation ano = PdfAnnotation.MakeAnnotation(a);

    var apDict = ano.GetAppearanceDictionary();
    if (apDict == null)
    {
        Console.WriteLine("No appearances.");
        continue;
    }
    foreach (PdfName key in apDict.KeySet())
    {
        Console.WriteLine("Appearance: {0}", key);
        PdfStream value = apDict.GetAsStream(key);
        if (value != null)
        {
            var text = ExtractAnnotationText(value);
            Console.WriteLine("Extracted Text: {0}", text);

            if (text != "")
            {
                var valueString = Encoding.ASCII.GetString(value.GetBytes());
                value.SetData(Encoding.ASCII.GetBytes(valueString.Replace(text, "COMMENT: " + text)));
            }
        }
    }
    masterDocPage.AddAnnotation(ano);
}

public static String ExtractAnnotationText(PdfStream xObject)
{
   PdfResources resources = new PdfResources(xObject.GetAsDictionary(PdfName.Resources));
   ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();

   PdfCanvasProcessor processor = new PdfCanvasProcessor(strategy);
   processor.ProcessContent(xObject.GetBytes(), resources);
   var text = strategy.GetResultantText();
   return text;
}
         
         */

        ///*
        // pdfDoc.GetPage(33).GetAnnotations().First().SetName(new PdfString("Hello"))
        // pdfDoc.GetPage(33).GetAnnotations().First(x => x.GetName().ToString() == "Hello")
        //*/
        //public void AddCitationToPdfDoc(String storageFileName, int physicalPage, Category cat, CitationCategory citCat, string rectsString)
        //{
        //    if (citCat == null)
        //        return;

        //    if (cat == null)
        //        citCat.CategoryId = Id.Empty;

        //    string storageFilePath = System.IO.Path.Combine(m_UserSettingsService.StorageFolder, storageFileName);

        //    List<(int page, int[] rects)> pageRects = ArrayStuff.ConvertStringToPagesAndArrays(rectsString);

        //    // Remove existing annot
        //    int firstPageNo = GetFirstPageFromPageRects(pageRects);
        //    bool annotExists = RemoveAnnotation(storageFileName, firstPageNo, citCat.CitationId.ToString());

        //    string tmpFileName = m_TempFileService.GetNewTmpFileName(storageFileName);
        //    PdfDocument pdfDoc = new PdfDocument(new PdfReader(storageFilePath), new PdfWriter(tmpFileName));

        //    //PdfAnnotation existingAnnot = pdfDoc.GetPage(firstPageNo).GetAnnotations().FirstOrDefault(x => x.GetName()?.ToString() == $"belpdf:{citCat.CitationId}");
        //    //bool annotExists = existingAnnot != null;
        //    //if (annotExists)
        //    //    RemoveAnnotation(pdfDoc, citCat.CitationId.ToString());

        //    if (!annotExists && pageRects.Any())
        //    {
        //        System.Drawing.Color c;
        //        c = m_UserSettingsService.PdfHighLightColor;
        //        Color color_highLight = new DeviceRgb(n(c.R), n(c.G), n(c.B));
        //        c = m_UserSettingsService.PdfHighLightColor;
        //        Color color_underLine = new DeviceRgb(n(c.R), n(c.G), n(c.B));

        //        foreach (var pageRect in pageRects)
        //        {
        //            //int currentPage = pageRects.First().page;
        //            int currentPage = pageRect.page;
        //            int currentRect = 0;
        //            bool notDone = true;
        //            do
        //            {
        //                PdfCanvas canvas1 = new PdfCanvas(
        //                    pdfDoc.GetPage(currentPage).NewContentStreamBefore(),
        //                    pdfDoc.GetPage(currentPage).GetResources(), pdfDoc);
        //                PdfCanvas canvas2 = new PdfCanvas(
        //                    pdfDoc.GetPage(currentPage).NewContentStreamAfter(),
        //                    pdfDoc.GetPage(currentPage).GetResources(), pdfDoc);

        //                Rectangle pageSize = pdfDoc.GetPage(physicalPage).GetPageSize();
        //                int pageHeight = (int)pageSize.GetHeight();
        //                int pageWidth = (int)pageSize.GetWidth();

        //                //canvas.SaveState();

        //                canvas1.SetFillColor(color_highLight);
        //                canvas2.SetFillColor(color_underLine);

        //                do //(int i = 0; i < rects.Length; i += 4)
        //                {
        //                    // Below text highlight
        //                    canvas1.Rectangle(pageRect.rects[currentRect], pageHeight - pageRect.rects[currentRect + 1] - pageRect.rects[currentRect + 3], pageRect.rects[currentRect + 2], pageRect.rects[currentRect + 3]);
        //                    canvas1.Fill();
        //                    // Above text underline
        //                    canvas2.Rectangle(pageRect.rects[currentRect], pageHeight - pageRect.rects[currentRect + 1] - pageRect.rects[currentRect + 3], pageRect.rects[currentRect + 2], 1);
        //                    canvas2.Fill();

        //                    if (currentRect + 5 > pageRect.rects.Length)
        //                    {
        //                        notDone = false;
        //                        break;
        //                    }

        //                    // Detect page break
        //                    int lastY = pageHeight - pageRect.rects[currentRect + 1] - pageRect.rects[currentRect + 3];
        //                    currentRect += 4;
        //                    int newY = pageHeight - pageRect.rects[currentRect + 1] - pageRect.rects[currentRect + 3];
        //                    if (lastY < newY)
        //                    {
        //                        currentPage++;
        //                        break;
        //                    }

        //                } while (true);
        //            } while (notDone);
        //        }
        //    }
        //    //canvas.RestoreState();

        //    // Add annotation
        //    //AddMarginCategoryTextAnnotation(pdfDoc, null, physicalPage, citCat.CitationId.ToString(), cat.Code + $" [{citCat.Weight}]", rectsString, null, );

        //    pdfDoc.Close();

        //    File.Delete(storageFilePath);
        //    File.Move(tmpFileName, storageFilePath);

        //}


        /// <summary>
        /// Returns margin x coords. Selects the widest margin of the left and the right one.
        /// </summary>
        /// <returns></returns>
        //private DekRange GetMargin(PdfDocument doc, int pageNo, string rectsString)
        //{
        //    PdfPage page = doc.GetPage(pageNo);
        //    Rectangle pagesize = page.GetPageSize();
        //    int[] rects = ArrayStuff.ConvertStringToArray(rectsString);
        //    var bounds = GetLeftAndRightBounds(rects);

        //    if (bounds.Stop > pagesize.GetWidth())
        //        bounds = new DekRange(bounds.Start, (int)pagesize.GetWidth());

        //    int leftSize = (int)(bounds.Start - pagesize.GetX());
        //    int rightSize = (int)(pagesize.GetX() + pagesize.GetWidth() - bounds.Stop);
        //    return leftSize > rightSize
        //        ? new DekRange((int)pagesize.GetX(), (int)pagesize.GetX() + leftSize)
        //        : new DekRange(bounds.Stop, rightSize);
        //}

    }
}
