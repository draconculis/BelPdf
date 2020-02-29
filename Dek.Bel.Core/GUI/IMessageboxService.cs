namespace Dek.Bel.Core.GUI
{
    public interface IMessageboxService
    {
        void Show(string message, string header = "Message");

        DekDialogResult ShowYesNo(string message, string header);

        /// <summary>
        /// Show message if UserSettingsService.ShowDebugMessages is true.
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);

        (DekDialogResult result, string path) OpenFileDialogForSave(string intitialFolderPath, string filter);
        (DekDialogResult result, string path) OpenFileDialogForOpen(string initialFolderPath, string filter);
        (DekDialogResult result, string path) OpenBrowseFolderDialog(string initialFolderPath);
    }
}