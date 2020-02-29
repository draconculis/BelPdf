namespace Dek.DB
{
    public interface IDBUpdater
    {
        void Upgrade(IDBService repo);
    }
}
