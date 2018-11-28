namespace Dek.Bel.UserSettings
{
    public interface IUserSettingsService
    {
        string StorageFolder { get; set; }
        string DBName { get; }
        string DBPath { get; }
    }
}