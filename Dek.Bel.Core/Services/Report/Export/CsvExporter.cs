using System;
using System.ComponentModel.Composition;
using System.IO;

namespace Dek.Bel.Core.Services.Report.Export
{
    [Export(typeof(IExporter))]
    public class CsvExporter : IExporter
    {
        public string Name => "Csv";

        public Stream Export()
        {
            throw new NotImplementedException();
        }
    }
}
