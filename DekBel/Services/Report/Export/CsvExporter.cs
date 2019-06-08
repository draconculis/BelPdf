using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Services.Report.Export
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
