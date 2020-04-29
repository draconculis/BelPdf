using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Dek.Bel
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = $"{fvi.FileMajorPart}.{fvi.FileMinorPart}.{fvi.FileBuildPart}";

            label_version.Text = $"Version {version}";
        }

        private void label_BelWebsite_Click(object sender, EventArgs e)
        {
            if (!(sender is Label label))
                return;

            OpenUrl(label.Text);
        }

        private void label_SumatraWebsite_Click(object sender, EventArgs e)
        {
            if (!(sender is Label label))
                return;

            OpenUrl(label.Text);
        }

        private void OpenUrl(string url)
        {
            Process.Start(url);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (!(sender is Label label))
                return;

            OpenUrl("https://github.com/draconculis/BelPdf/issues");
        }

        private void label_MouseEnter(object sender, EventArgs e)
        {
            if (!(sender is Label label))
                return;

            label.ForeColor = Color.CornflowerBlue;
        }

        private void label_MouseLeave(object sender, EventArgs e)
        {
            if (!(sender is Label label))
                return;

            label.ForeColor = Color.Black;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label_license_Click(object sender, EventArgs e)
        {
            if (!(sender is Label label))
                return;

            OpenUrl("https://www.gnu.org/licenses/gpl-3.0.en.html");

        }
    }
}
