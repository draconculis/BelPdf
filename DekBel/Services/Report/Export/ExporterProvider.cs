using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Services.Report.Export.Models
{
    [Export]
    public class ExporterProvider
    {
        [ImportMany] public List<IExporter> m_Exporters;

        public List<string> Names => m_Exporters?.Select(x => x.Name).ToList();

        public IExporter Provide(string name)
        {
            return m_Exporters?.SingleOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
