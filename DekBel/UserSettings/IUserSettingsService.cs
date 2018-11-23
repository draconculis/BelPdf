namespace Dek.Bel.UserSettings
{
    public interface IUserSettingsService
    {
        string ProgramDataPath { get; set; }
        string DBName { get; set; }
        string DBPath { get; }
    }
}