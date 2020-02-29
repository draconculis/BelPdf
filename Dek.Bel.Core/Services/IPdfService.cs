namespace Dek.Bel.Core.Services
{
    public interface IPdfService
    {
        void RecreateTheWholeThing(ModelsForViewing vm, VolumeService volumeService);
    }
}