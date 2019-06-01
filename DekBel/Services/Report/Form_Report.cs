using Dek.Bel.Cls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel.Services.Report
{
    public partial class Form_Report : Form
    {

        [Import] public ReportService m_ReportService { get; set; }
        [Import] public IMessageboxService m_MessageBoxService { get; set; }

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
            source.DataSource = m_ReportService.Report;
            dataGridView1.DataSource = source;

            label_time.Text = $"Took: {milliseconds / 1000.0} seconds";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
