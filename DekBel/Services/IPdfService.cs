using Dek.Bel.DB;

namespace Dek.Bel.Services
{
    public interface IPdfService
    {
        void RecreateTheWholeThing(ModelsForViewing vm, VolumeService volumeService);
    }
}