using BelManagedLib;
using Dek.Bel.Services;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Services
{
    [Export]
    public class PdfService
    {
        public IUserSettingsService m_UserSettingsService;

        [ImportingConstructor]
        public PdfService(IUserSettingsService userSettingsService)
        {
            m_UserSettingsService = userSettingsService;
        }

        public void Test(EventData data)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(data.FilePath), new PdfWriter(data.FilePath));


            

            pdfDoc.Close();
        }
    }
}
