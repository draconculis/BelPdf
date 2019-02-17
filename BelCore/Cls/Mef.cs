using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel.Cls
{
    public class Mef
    {
        public static void Initialize(object obj) => InitializeInternal(obj);

        private static void InitializeInternal(object obj)
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(BelGui).Assembly));

            var container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(obj);
            }
            catch (CompositionException compositionException)
            {
                MessageBox.Show($"{compositionException}", "Composition error");
            }

        }
    }
}
