using System.Collections.Generic;

namespace Dek.Bel.Core.Services.Report.Export
{
    public interface IExporter
    {
        string Name { get; }
        string Export(string title, List<string> colNames, List<List<string>> data);
    }
}
