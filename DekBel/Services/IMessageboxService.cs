using System.Windows.Forms;

namespace Dek.Bel.Services
{
    public interface IMessageboxService
    {
        void Show(string message, string header = "Message");
        DialogResult ShowYesNo(string message, string header);
        void Debug(string message);
    }
}