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
        public static void ShowMessage(string message, string header)
        {
            MessageBox.Show(message, header);
        }

        public void Show(string message, string header)
        {
            MessageBox.Show(message, header);
        }
    }
}
