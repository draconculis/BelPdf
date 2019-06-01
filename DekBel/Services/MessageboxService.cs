using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel.Services
{
    [Export(typeof(IMessageboxService))]
    public class MessageboxService : IMessageboxService
    {

        [Import] public IUserSettingsService m_UserSettingsService;

        public static void ShowMessage(string message, string header)
        {
             MessageBox.Show(message, header);
        }

        public void Show(string message, string header)
        {
            MessageBox.Show(message, header);
        }

        public DialogResult ShowYesNo(string message, string header)
        {
            return MessageBox.Show(message, header, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public void Debug(string message)
        {
            if(m_UserSettingsService.ShowDebugMessages)
                MessageBox.Show(message);
        }

    }
}
