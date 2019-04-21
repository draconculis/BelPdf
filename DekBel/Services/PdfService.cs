using BelManagedLib;
using Dek.Bel.Services;
using Dek.Bel.Cls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Colors;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using Dek.Cls;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Annot;
using Dek.Bel.Models;
using iText.Kernel.Pdf.Filespec;
using iText.Kernel.Pdf.Action;
using iText.Layout.Borders;
using iText.Kernel.Pdf.Colorspace;

namespace Dek.Bel.Services
{
    [Export]
    public class PdfService
    {
        [Import] public IUserSettingsService m_UserSettingsService { get; set; }
        [Import] public TempFileService m_TempFileService { get; set; }

        public void Test(EventData data)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(data.FilePath), new PdfWriter(data.FilePath));




            pdfDoc.Close();
        }

        public void ManipulatePdf(String storageFileName, string rectsString)
        {
            storageFileName = @"C:\Users\stimo\Documents\BelPdf\1_Refining_the_Definitions_of_Amulet_Phy.bel.1_A.pdf";
            string tmpFileName = @"C:\Users\stimo\Documents\BelPdf\1_Refining_the_Definitions_of_Amulet_Phy.bel.1_B.pdf";
            int[] rects = ArrayStuff.ConvertStringToArray(rectsString);

            PdfDocument pdfDoc = new PdfDocument(new PdfReader(storageFileName), new PdfWriter(tmpFileName));
            PdfCanvas canvas = new PdfCanvas(
                pdfDoc.GetFirstPage().NewContentStreamBefore(),
                pdfDoc.GetFirstPage().GetResources(), pdfDoc);

            canvas.SaveState();

            canvas.SetFillColor(ColorConstants.CYAN);
            int canvasHeight = (int)pdfDoc.GetPage(1).GetPageSize().GetHeight();
            int canvasWidth = (int)pdfDoc.GetPage(1).GetPageSize().GetWidth();

            for (int i = 0; i < rects.Length; i += 4)
            {
                canvas.Rectangle(rects[i], canvasHeight - rects[i + 1] - rects[i + 3], rects[i + 2], rects[i + 3]);
            }

            //canvas.Rectangle(36, 786, 66, 16);

            canvas.Fill();

            canvas.RestoreState();

            pdfDoc.Close();

        }

        public void AddCitationToPdfDoc(String storageFileName, int physicalPage, Category cat, CitationCategory citCat, string rectsString)
        {
            if(cat == null || citCat == null)
            {
                MessageboxService.ShowMessage("Citation must be categorized before adding annotation to pdf.", "Category not set");
                return;
            }


            string storageFilePath = System.IO.Path.Combine(m_UserSettingsService.StorageFolder, storageFileName);
            string tmpFileName = m_TempFileService.GetNewTmpFileName(storageFileName);

            PdfDocument pdfDoc = new PdfDocument(new PdfReader(storageFilePath), new PdfWriter(tmpFileName));
            PdfCanvas canvas1 = new PdfCanvas(
                pdfDoc.GetPage(physicalPage).NewContentStreamBefore(),
                pdfDoc.GetPage(physicalPage).GetResources(), pdfDoc);
            PdfCanvas canvas2 = new PdfCanvas(
                pdfDoc.GetPage(physicalPage).NewContentStreamAfter(),
                pdfDoc.GetPage(physicalPage).GetResources(), pdfDoc);

            Rectangle pageSize = pdfDoc.GetPage(physicalPage).GetPageSize();
            int pageHeight = (int)pageSize.GetHeight();
            int pageWidth = (int)pageSize.GetWidth();

            //canvas.SaveState();

            Color color1 = new DeviceRgb(0.5f, 1.0f, 1.0f);
            Color color2 = new DeviceRgb(1.0f, 0.4f, 0.4f);
            canvas1.SetFillColor(color1);
            canvas2.SetFillColor(color2);
            int[] rects = ArrayStuff.ConvertStringToArray(rectsString);
            for (int i = 0; i < rects.Length; i += 4)
            {
                canvas1.Rectangle(rects[i], pageHeight - rects[i + 1] - rects[i + 3], rects[i + 2], rects[i + 3]);
                canvas1.Fill();
                canvas2.Rectangle(rects[i], pageHeight - rects[i + 1] - rects[i + 3], rects[i + 2], 1);
                canvas2.Fill();
            }

            //canvas.RestoreState();

            // Add annotation
            AddMarginCategoryTextAnnotation(pdfDoc, physicalPage, citCat.CitationId.ToString(), cat.Code + $" [{citCat.Weight}]", rectsString);

            pdfDoc.Close();

            File.Delete(storageFilePath);
            File.Move(tmpFileName, storageFilePath);

        }

        public void AddMarginCategoryTextAnnotation(PdfDocument pdfDoc, int physicalPage, string id, string text, string rectsString)
        {
            int boxWidth = 56;
            int boxHeight = 13;
            int boxLeft = 11;
            int[] rects = ArrayStuff.ConvertStringToArray(rectsString);
            int yCoord = rects[1];
            Rectangle pageSize = pdfDoc.GetPage(physicalPage).GetPageSize();
            int pageHeight = (int)pageSize.GetHeight();
            int pageWidth = (int)pageSize.GetWidth();
            //Rectangle rect = new Rectangle(boxLeft, pageHeight - yCoord - boxHeight, boxWidth, boxHeight);

            // Page number labels
            pdfDoc.GetFirstPage().SetPageLabel(PageLabelNumberingStyle.DECIMAL_ARABIC_NUMERALS, null);

            //DekRange margin = GetMargin(pdfDoc, physicalPage, rectsString);
            PdfPage page = pdfDoc.GetPage(physicalPage);
            //PdfCanvas pdfCanvas = new PdfCanvas(pdfDoc, physicalPage);
            //Canvas canvas = new Canvas(pdfCanvas, pdfDoc, rect);
            //Canvas canvas = new Canvas(pdfCanvas, pdfDoc, rect);

            // Action
            PdfFileSpec spec = PdfFileSpec.CreateExternalFileSpec(pdfDoc, $"belpdf:{id}");
            PdfAction action = PdfAction.CreateLaunch(spec);

            //
            PdfFont font = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.TIMES_ROMAN);

            // Paragraph
            //var paragraph = new iText.Layout.Element.Paragraph(new Link(text, action));
            //paragraph.SetBorder(new SolidBorder(1));

            //paragraph.SetFont(font).SetFontSize(9);

            //canvas.Add(paragraph);

            var paragraph1 = new iText.Layout.Element.Paragraph()
                .Add(new Link(text, action))
                .SetStrokeColor(ColorConstants.BLACK)
                .SetFontColor(ColorConstants.BLACK)
                .SetFixedPosition(boxLeft, pageHeight - yCoord - boxHeight, boxWidth) // NB that height is variable
                //.SetBorder(new SolidBorder(0.5f))
                .SetFont(font)
                .SetFontSize(9);
            Document doc = new Document(pdfDoc);
            doc.Add(paragraph1);

            //canvas.Rectangle(rect);
            //canvas.Stroke();

            //PdfAnnotation ann = new PdfTextAnnotation(rect);
        }

        /// <summary>
        /// Returns margin x coords. Selects the widest margin of the left and the right one.
        /// </summary>
        /// <returns></returns>
        private DekRange GetMargin(PdfDocument doc, int pageNo, string rectsString)
        {
            PdfPage page = doc.GetPage(pageNo);
            Rectangle pagesize = page.GetPageSize();
            int[] rects = ArrayStuff.ConvertStringToArray(rectsString);
            var bounds = GetLeftAndRightBounds(rects);

            if (bounds.Stop > pagesize.GetWidth())
                bounds = new DekRange(bounds.Start, (int)pagesize.GetWidth());

            int leftSize = (int)(bounds.Start - pagesize.GetX());
            int rightSize = (int)(pagesize.GetX() + pagesize.GetWidth() - bounds.Stop);
            return leftSize > rightSize
                ? new DekRange((int)pagesize.GetX(), (int)pagesize.GetX() + leftSize)
                : new DekRange(bounds.Stop, rightSize);
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
            for (int i = 0 ; i < rects.Count() / 4 ; i++)
            {
                if (rects[i] < min)
                    min = rects[i];
                if (rects[i] + rects[i + 2] > max)
                    max = rects[i] + rects[i + 2];
            }

            return new DekRange(min, max);
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



    }
}
