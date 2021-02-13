using System.Drawing;

namespace Dek.Bel.Core.Services
{
    public interface IUserSettingsService
    {
        bool ShowDebugMessages { get; set; }
        string StorageFolder { get; set; }
        string LastSelectedDatabaseFile { get; set; }

        string DBName { get; }
        string DBPath { get; }
        string DBPathBeta { get; }
        string ReportsFolder { get; set; }
        string LatestExportSaveFolder { get; set; }

        string DeselectionMarker { get; }
        Font CitationFont { get; set; }
        bool BoldEmphasis { get; set; }
        bool UnderlineEmphasis { get; set; }
        Color PdfHighLightColor { get; set; }
        Color PdfUnderlineColor { get; set; }

        // Margin stuff
        Color PdfMarginBoxColor { get; set; }
        int PdfMarginBoxWidth { get; set; }
        int PdfMarginBoxHeight { get; set; }
        int PdfMarginBoxMargin { get; set; }
        float PdfMarginBoxBorder { get; set; }
        string PdfMarginBoxVisualMode { get; set; }
        bool PdfMarginBoxRightMargin { get; set; }
        string PdfMarginBoxFont { get; set; }
        float PdfMarginBoxFontSize { get; set; }



        bool AutoWritePdfOnClose { get; set; }
    }
}