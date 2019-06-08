using Dek.Bel.Cls;
using System;
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
            StringBuilder sb = new StringBuilder();

            sb.Append($"<html><head><title></title></head><body>");
            sb.Append(Environment.NewLine);
            sb.Append("<table>");
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
                    sb.Append(row.Cells[col.HeaderText].Value as string);
                    sb.Append("</td>" + Environment.NewLine);
                }
                sb.Append("</tr>");
                sb.Append(Environment.NewLine);

            }

            sb.Append("</table></body></html>");

            string filePath = Path.Combine(m_UserSettingsService.StorageFolder, "\\report.html");
            File.WriteAllText(filePath, sb.ToString());

            System.Diagnostics.Process.Start(filePath);
        }
    }
}
