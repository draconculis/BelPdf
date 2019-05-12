using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.Models
{
    public class MetaDbModel<T>
    {
        List<string> ExcludedFileds { get; set; }
        MetaModel<T> Model { get; set; }

        public string GenerateSql()
        {
            return "";
        }

        public string Scan()
        {

            // Scan main model for keys
            // Collect fields

            // Scan models for 
            // 1. Single value tables. Model contains Key MainModelId.
            // 2. Multi value tables. Model does not contain MainModelId. Assume table ModelMainModel or MainModelModel exists.
            //    Then ModelMainModelId or MainModelModelId exists.
            //    MultiValue field returned looks like {"Id1", "val1", "abc"}{"Id2", "val2", "abc"}{"Id3", "val3", "abc"}

            // Returned data is <ModelId, a b c><<MultiValue1 abc><MutliValue2 abc>><SingleValue abc> etc
            // Keys not present in single value tables.
            // Keys present in MultiValue tables.


            return "";
        }
    }
}
