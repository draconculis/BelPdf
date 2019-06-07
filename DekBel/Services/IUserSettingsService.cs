using System.Drawing;

namespace Dek.Bel.Services
{
    public interface IUserSettingsService
    {
        bool ShowDebugMessages { get; set; }
        string StorageFolder { get; set; }
        string DBName { get; }
        string DBPath { get; }
        string DeselectionMarker { get; }
        Font CitationFont { get; set; }
        bool BoldEmphasis { get; set; }
        bool UnderlineEmphasis { get; set; }
        Color PdfHighLightColor { get; set; }
        Color PdfUnderlineColor { get; set; }
        bool AutoWritePdfOnClose { get; set; }
    }
}