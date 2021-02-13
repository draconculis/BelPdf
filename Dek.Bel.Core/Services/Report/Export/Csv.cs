using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Dek.Bel.Core.Services.Report.Export
{
    [Export(typeof(IExporter))]
    public class Csv : IExporter
    {
        public string Name => "Csv";
        [Import] public IUserSettingsService m_UserSettingsService { get; set; }

        private string delimiter = ", ";

        string IExporter.Export(string title, List<string> colNames, List<List<string>> data)
        {
            StringBuilder sb = new StringBuilder();
            // Do not show emphasis column
            List<string> filteredColNames = colNames.Where(x => x.ToLower() != "emphasis").ToList();

            // Title is ignored

            // Headers
            foreach (string colName in filteredColNames)
            {
                sb.Append("\"" + colName + "\"" + delimiter);
            }
            sb.Append(Environment.NewLine);

            int rowCount = data.Count;
            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
            {
                foreach (string colName in filteredColNames)
                {
                    string cellValue = Helper.GetDataValue(rowIdx, colName, colNames, data)
                        .Replace("\n", " ")
                        .Replace("\r", " ")
                        .Replace("    ", " ")
                        .Replace("   ", " ")
                        .Replace("  ", " ")
                        .Replace("\"", "‟");

                    sb.Append("\"" + cellValue + "\"" + delimiter);
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

    }
}
