namespace Dek.Bel.DB
{
    public interface IDBUpdater
    {
        void Upgrade(IDBService repo);
    }
}
