using System.Drawing;

namespace Dek.Bel.Services
{
    public interface IUserSettingsService
    {
        string StorageFolder { get; set; }
        string DBName { get; }
        string DBPath { get; }
        string DeselectionMarker { get; }
        Font CitationFont { get; set; }
        bool BoldEmphasis { get; set; }
        bool UnderlineEmphasis { get; set; }
    }
}