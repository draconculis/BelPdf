using Dek.Bel.Core.Models;
using Dek.Bel.Core.Services;
using Dek.Cls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace Dek.Bel.Services
{
    public partial class FormSeries : Form
    {
        public Series SelectedSeries = null;
        private IEnumerable<Series> m_Series;
        [Import] SeriesService m_SeriesService;
        private IEnumerable<Volume> m_VolumesInSeries;

        public FormSeries(Id volumeId)
        {
            if (m_SeriesService == null)
                Mef.Compose(this);

            InitializeComponent();

            m_Series = m_SeriesService.GetAllSeries();

            listBox1.DataSource = m_Series;

            Series series = m_SeriesService.GetSeriesForVolume(volumeId);
            SelectSeries(series);
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            SelectedSeries = null;
            Close();
        }

        /// <summary>
        /// Load Volumes and set textboxes
        /// </summary>
        private void SelectSeries(Series series)
        {
            listBox2.DataSource = null;

            if (series == null)
            {
                listBox2.DataSource = null;

                textBox_SeriesName.Text = "";
                textBox_SeriesNotes.Text = "";
                return;
            }

            m_VolumesInSeries = m_SeriesService.GetVolumesInSeries(series.Id);
            listBox2.DataSource = m_VolumesInSeries;

            textBox_SeriesName.Text = series.Name;
            textBox_SeriesNotes.Text = series.Notes;

            if(listBox1.Items.Count > 0)
            {
                int idx = listBox1.FindStringExact(series.Name);
                listBox1.SelectedIndex = idx;
            }
        }

        /// <summary>
        /// Update series
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_SeriesName_Leave(object sender, EventArgs e)
        {
            if (!m_Series.Any())
                return;

            if (listBox1.SelectedIndex < 0)
                return;

            var series = (Series)listBox1.Items[listBox1.SelectedIndex];
            if (series == null)
                return;

            if (m_Series.Where(x => x.Id != series.Id).Any(s => s.Name == textBox_SeriesName.Text))
            {
                MessageBox.Show("Bad Name!", "Name must be unique.");
                textBox_SeriesName.Text = series.Name;
                return;
            }

            series.Name = textBox_SeriesName.Text;
            series.Notes = textBox_SeriesNotes.Text;

            m_SeriesService.InsertOrUpdateSeries(series);

            listBox1.DataSource = null;
            m_Series = m_SeriesService.GetAllSeries();
            listBox1.DataSource = m_Series;

            SelectSeries(series);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is ListBox listBox))
                return;

            if (listBox1.SelectedIndex < 0)
                return;

            SelectSeries((Series)listBox1.Items[listBox1.SelectedIndex]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedSeries = null;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button_Select_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                SelectedSeries = (Series)listBox1.Items[listBox1.SelectedIndex];

            Close();
        }

        private void button_Create_Click(object sender, EventArgs e)
        {
            Series newSeries = m_SeriesService.CreateSeries(GenerateNewSeriesName(), "");

            listBox1.DataSource = null;
            m_Series = m_SeriesService.GetAllSeries();
            listBox1.DataSource = m_Series;

            SelectSeries(newSeries);
        }

        private void button_DeleteSeries_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;

            Series deleteSeries = (Series)listBox1.Items[listBox1.SelectedIndex];
            if (deleteSeries == null)
                return;
            
            if (MessageBox.Show(this, $"Delete series \"{deleteSeries.Name}\"?{Environment.NewLine}This can not be undone.", "Delete Series", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                m_SeriesService.DeleteSeries(deleteSeries);
                listBox1.DataSource = null;
                m_Series = m_SeriesService.GetAllSeries();
                listBox1.DataSource = m_Series;
            }
        }


        string GenerateNewSeriesName()
        {
            string nameBase = "New Series";
            string newName = nameBase;
            int count = 1;
            while(m_Series.Any(s => s.Name == newName))
                newName = $"{nameBase} {count++}";

            return newName;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            return;
            
            SelectedSeries = (Series)listBox1.Items[listBox1.SelectedIndex];
            Close();
        }


        private void textBox_SeriesName_KeyUp(object sender, KeyEventArgs e)
        {
            textBox_SeriesName.BackColor = Color.White;

            if (!(sender is TextBox tb))
                return;

            if(!tb.Focused)
                return;

            if (!m_Series.Any() || listBox1.Items.Count < 1 || listBox1.SelectedItem == null || listBox1.SelectedIndex < 0)
                return;

            Series series = (Series)listBox1.Items[listBox1.SelectedIndex];
            IEnumerable<Series> otherSeries = m_Series.Where(x => x.Id != series.Id);

            if (otherSeries.Any(x => x.Name == tb.Text))
            {
                textBox_SeriesName.BackColor = Color.LightPink;
            }

        }
    }
}
