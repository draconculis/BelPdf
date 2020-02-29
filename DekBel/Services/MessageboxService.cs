using Dek.Bel.Core.GUI;
using Dek.Bel.Core.Services;
using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace Dek.Bel.Services
{
    [Export(typeof(IMessageboxService))]
    public class MessageboxService : IMessageboxService
    {
        [Import] public IUserSettingsService m_UserSettingsService;

        public void Show(string message, string header)
        {
            MessageBox.Show(message, header);
        }

        public DekDialogResult ShowYesNo(string message, string header)
        {
            return (DekDialogResult)MessageBox.Show(message, header, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public void Debug(string message)
        {
            if(m_UserSettingsService.ShowDebugMessages)
                MessageBox.Show(message);
        }

        public (DekDialogResult result, string path) OpenFileDialogForSave(string intitialFolderPath, string filter)
        {
            throw new System.NotImplementedException();
        }

        public (DekDialogResult result, string path) OpenFileDialogForOpen(string initialFolderPath, string filter)
        {
            throw new System.NotImplementedException();
        }

        public (DekDialogResult result, string path) OpenBrowseFolderDialog(string initialFolderPath)
        {
            throw new System.NotImplementedException();
        }
    }
}
