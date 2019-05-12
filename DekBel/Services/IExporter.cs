using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Services
{
    public interface IExporter
    {
        string Name { get; }
        Stream Export();
    }
}
