using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml; // EPPlus

namespace Dek.Bel.Services.Report.Export
{
    [Export(typeof(IExporter))]
    public class HtmlExporter : IExporter
    {
        public string Name => "Html";

        public Stream Export()
        {
            throw new NotImplementedException();
        }
    }
}
