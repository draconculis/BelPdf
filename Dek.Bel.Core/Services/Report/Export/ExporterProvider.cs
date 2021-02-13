using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Dek.Bel.Core.Services.Report.Export
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
