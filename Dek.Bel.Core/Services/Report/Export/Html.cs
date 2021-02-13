using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Dek.Cls;

namespace Dek.Bel.Core.Services.Report.Export
{
    [Export(typeof(IExporter))]
    public class Html : IExporter
    {
        public string Name => "Html";

        [Import] public IUserSettingsService m_UserSettingsService { get; set; }

        string IExporter.Export(string title, List<string> colNames, List<List<string>> data)
        {
            StringBuilder sb = new StringBuilder();
            // Do not show emphasis column
            List<string> filteredColNames = colNames.Where(x => x.ToLower() != "emphasis").ToList();

            sb.Append($"<html><head><title>{title}</title>");
            AppendReportTableStyle(sb);
            sb.Append("</head><body>");
            // Page title
            sb.Append($"<h1>{title}</h1>");
            sb.Append(Environment.NewLine);
            sb.Append("<table id=\"report\">");
            // Headers
            sb.Append("<tr>");
            sb.Append(Environment.NewLine);
            foreach (string colName in filteredColNames)
            {
                sb.Append("<th>");
                sb.Append(colName);
                sb.Append("</th>" + Environment.NewLine);
            }
            sb.Append("</tr>");
            sb.Append(Environment.NewLine);

            int rowCount = data.Count;
            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
            {
                sb.Append("<tr>");
                sb.Append(Environment.NewLine);

                foreach (string colName in filteredColNames)
                {
                    sb.Append("<td>");

                    string cellValue = Helper.GetDataValue(rowIdx, colName, colNames, data);

                    if (Helper.IsCitation(colName))
                    {
                        string emphasis = Helper.GetDataValue(rowIdx, nameof(ReportModel.Emphasis), colNames, data);
                        HandleCitation3(sb, cellValue, emphasis); // Builds export html for citation
                    }
                    else
                        sb.Append(cellValue);

                    sb.Append("</td>" + Environment.NewLine);
                }
                sb.Append("</tr>");
                sb.Append(Environment.NewLine);
            }

            sb.Append("</table></body></html>");

            return sb.ToString();
        }

        private void HandleCitation3(StringBuilder sb, string text, string emphasisString)
        {
            if (text.IsNullOrWhiteSpace())
                return;

            string s = text.Replace("\r\n", "\r").Replace("\n", "\r");

            List<DekRange> emphasis = new List<DekRange>();
            emphasis.LoadFromText(emphasisString);

            bool boldEmphasis = m_UserSettingsService.BoldEmphasis;
            bool underlineEmphasis = m_UserSettingsService.UnderlineEmphasis;

            StringBuilder htmlStringBuilder = new StringBuilder();
            bool inEmphasis = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (emphasis.ContainsInteger(i) && !inEmphasis)
                {
                    if (boldEmphasis)
                        htmlStringBuilder.Append("<b>");
                    if (underlineEmphasis)
                        htmlStringBuilder.Append("<i>");
                    inEmphasis = true;
                }
                if (!emphasis.ContainsInteger(i) && inEmphasis)
                {
                    if (underlineEmphasis)
                        htmlStringBuilder.Append("</i>");
                    if (boldEmphasis)
                        htmlStringBuilder.Append("</b>");

                    inEmphasis = false;
                }

                htmlStringBuilder.Append(s[i]);
            }

            var ret = htmlStringBuilder.ToString();
            ret = ret.Replace("\r", "\r\n");

            sb.Append(ret);
        }

        private void AppendReportTableStyle(StringBuilder sb)
        {
            sb.Append(
                @"<style>
                    #report {
                    font-family: ""Trebuchet MS"", Arial, Helvetica, sans-serif;
                    border-collapse: collapse;
                    width: 200%;
                }

                #report td, #report th {
                    border: 1px solid #ddd;
                    padding: 8px;
                    vertical-align: top;
                }

                #report tr:nth-child(even){background-color: #f2f2f2;}

                #report tr:hover {background-color: #ffe;}

                #report th {
                    padding-top: 12px;
                    padding-bottom: 12px;
                    text-align: left;
                    background-color: #99ffcc;
                    color: Black;
                }
                </style>"
                );
        }

    }
}
