using System.IO;

namespace Dek.Bel.Core.Services.Report.Export
{
    public interface IExporter
    {
        string Name { get; }
        Stream Export();
    }
}
