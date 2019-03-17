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

        public void AddAnnotation(String storageFileName, string rectsString)
        {
            string storageFilePath = Path.Combine(m_UserSettingsService.StorageFolder, storageFileName);
            string tmpFileName = m_TempFileService.GetNewTmpFileName(storageFileName);
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

            File.Delete(storageFileName);
            File.Move(tmpFileName, storageFileName);

        }
    }
}
