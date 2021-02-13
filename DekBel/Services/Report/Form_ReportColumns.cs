using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.DataGrid;

namespace Dek.Bel.Services.Report
{
    public partial class Form_ReportColumns : Form
    {
        private SfDataGrid sfdg;

        public Form_ReportColumns(SfDataGrid sfDataGrid)
        {
            InitializeComponent();
            sfdg = sfDataGrid;
            foreach (GridColumn col in sfdg.Columns)
            {
                if (col.HeaderText == "Emphasis")
                {
                    col.Visible = false;
                    continue;
                }

                ListViewItem item = new ListViewItem();
                item.Checked = col.Visible;
                item.Text = col.HeaderText;
                listView1.Items.Add(item);
            }

            listView1.ItemChecked += ListView1_ItemChecked;
        }

        private void ListView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            try
            {
                sfdg.Columns[e.Item.Text].Visible = e.Item.Checked;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{e.Item.Text}: {e.Item.Checked}{Environment.NewLine}{ex.Message}", "Exception when setting column visibility");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            listView1.ItemChecked -= ListView1_ItemChecked;
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            SetAll(true);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            SetAll(false);
        }

        private void SetAll(bool check)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = check;
            }
        }
    }
}
