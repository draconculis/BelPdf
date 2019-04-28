namespace Dek.Bel.Services
{
    public interface IMessageboxService
    {
        void Show(string message, string header = "Message");
    }
}