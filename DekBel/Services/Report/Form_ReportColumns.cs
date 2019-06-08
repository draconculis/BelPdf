using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel.Services.Report
{
    public partial class Form_ReportColumns : Form
    {
        private DataGridView dgv;

        public Form_ReportColumns(DataGridView dataGridView)
        {
            InitializeComponent();
            dgv = dataGridView;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                ListViewItem item = new ListViewItem();
                item.Checked = col.Visible;
                item.Text = col.HeaderText;
                listView1.Items.Add(item);
            }

            listView1.ItemChecked += ListView1_ItemChecked;
        }

        private void ListView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //MessageBox.Show(e.Item.Text + $": {e.Item.Checked}");
            try
            {
                dgv.Columns[e.Item.Text].Visible = e.Item.Checked;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{e.Item.Text}: {e.Item.Checked}{Environment.NewLine}{ex.Message}", "Exception when setting column visibility");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
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
