using Dek.Bel.Cls;
using Dek.Cls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Dek.Bel.Services.Report
{
    public partial class Form_Report : Form
    {

        [Import] public ReportService m_ReportService { get; set; }
        [Import] public IMessageboxService m_MessageBoxService { get; set; }
        [Import] public IUserSettingsService m_UserSettingsService { get; set; }

        public Form_Report()
        {
            InitializeComponent();
            Mef.Initialize(this);

            long milliseconds = 0;
            try
            {
                milliseconds = m_ReportService.GenerateReport();
            }
            catch (Exception aex)
            {
                m_MessageBoxService.Show($"Exception: {aex.Message}", "Exception when generating report");
            }
            var source = new BindingSource();
            source.DataSource = m_ReportService.Filtered;
            dataGridView1.DataSource = source;

            // Hide some columns per default
            dataGridView1.Columns[nameof(ReportModel.Emphasis)].Visible = false;

            label_time.Text = $"Took: {milliseconds / 1000.0} seconds";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CheckBox_autoSizeRows_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeRows(
                checkBox_autoSizeRows.Checked
                ? DataGridViewAutoSizeRowsMode.DisplayedCells
                : DataGridViewAutoSizeRowsMode.None);

            dataGridView1.Invalidate(true);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Predicate<ReportModel> filter = x => x.MainCategory.StartsWith(textBox1.Text);


            //var bd = (BindingSource)dataGridView1.DataSource;
            //var dt = (DataTable)bd.DataSource;
            //dt.DefaultView.RowFilter = string.Format($"MainCategory Like '%{textBox1.Text}%'");

            m_ReportService.Filter = filter; 
            var source = new BindingSource();
            source.DataSource = m_ReportService.Filtered;
            dataGridView1.DataSource = source;
            dataGridView1.Refresh();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            ShowColumnSelector();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.Visible = true;
            }

        }

        private void ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void ToolStripMenuItem_columns_Click(object sender, EventArgs e)
        {
            ShowColumnSelector();
        }

        private void ShowColumnSelector()
        {
            Form_ReportColumns formCols = new Form_ReportColumns(dataGridView1);
            formCols.ShowDialog();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            string title = $"Bel Report {DateTime.Now.ToSaneStringShort()}";
            string fileName = $"Bel_Report_{DateTime.Now.ToCompactStringShort()}.html";

            StringBuilder sb = new StringBuilder();

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
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                sb.Append("<th>");
                sb.Append(col.HeaderText);
                sb.Append("</th>" + Environment.NewLine);
            }
            sb.Append("</tr>");
            sb.Append(Environment.NewLine);

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                sb.Append("<tr>");
                sb.Append(Environment.NewLine);

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    sb.Append("<td>");

                    if (col.HeaderText == nameof(ReportModel.Citation))
                    {
                        HandleCitation3(sb, row.Cells[col.HeaderText].Value.ToString(), row.Cells[nameof(ReportModel.Emphasis)].Value.ToString());
                    }
                    else
                        sb.Append(row.Cells[col.HeaderText].Value.ToString());

                    sb.Append("</td>" + Environment.NewLine);
                }
                sb.Append("</tr>");
                sb.Append(Environment.NewLine);

            }

            sb.Append("</table></body></html>");

            string filePath = Path.Combine(m_UserSettingsService.StorageFolder, fileName);
            File.WriteAllText(filePath, sb.ToString());

            System.Diagnostics.Process.Start(filePath);
        }

        private void HandleCitation3(StringBuilder sb, string text, string emphasisString)
        {
            if (text.IsNullOrWhiteSpace())
                return;

            string s = text.Replace("\r\n", "\r").Replace("\n", "\r");

            List<DekRange> emphasis = new List<DekRange>();
            emphasis.LoadFromText(emphasisString);

            if (emphasis == null)
                emphasis = new List<DekRange>();

            bool boldEmphasis = m_UserSettingsService.BoldEmphasis;
            bool underlineEmphasis = m_UserSettingsService.UnderlineEmphasis;

            StringBuilder htmlStringBuilder = new StringBuilder();
            //rtfbuilder.Append(@"{\rtf1\ansi ");
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
