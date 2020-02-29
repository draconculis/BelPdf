using Dek.Bel.Core.Services.Toaster;
using System.ComponentModel.Composition;

namespace Dek.Bel.Services.Toaster
{
    [Export(typeof(IToasterService))]
    public class ToasterService : IToasterService
    {
        public void ShowToast(string message, string text)
        {
            var toast = new Toast(message, text);
            toast.Show();
        }
    }
}
