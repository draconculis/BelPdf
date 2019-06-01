using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml; // EPPlus

namespace Dek.Bel.Services.Export
{
    [Export(typeof(IExporter))]
    public class ExcelExporter : IExporter
    {
        public string Name => "Excel";

        public Stream Export()
        {
            throw new NotImplementedException();
        }
    }
}
