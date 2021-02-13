using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dek.Bel.Core.Services;

namespace Dek.Bel.Core.Services.Report.Export
{
    internal class Helper
    {
        internal static bool IsCitation(string colName)
        {
            return colName == nameof(ReportModel.Citation);
        }

        internal static string GetDataValue(int rowIdx, string colName, List<string> headers, List<List<string>> data)
        {
            int colIdx = headers.FindIndex(a => string.Compare(a, colName, true) == 0);
            try
            {
                return data[rowIdx][colIdx];
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
