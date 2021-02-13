using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml; // EPPlus

namespace Dek.Bel.Core.Services.Report.Export
{
    [Export(typeof(IExporter))]
    public class Excel : IExporter
    {
        public string Name => "Excel";

        string IExporter.Export(string title, List<string> colNames, List<List<string>> data)
        {
            return string.Empty;
        }
    }
}
