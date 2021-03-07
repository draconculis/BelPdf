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
using Dek.Bel.Core.Models;
using Dek.Bel.Core.Services;
using Dek.Cls;

namespace Dek.Bel.Services.Report
{
    public partial class Form_ReportCategory : Form
    {
        [Import] ICategoryService m_CategoryService { get; set; }

        public Id CitationId { get; }

        public Form_ReportCategory(Id citationId, int X, int Y)
        {
            CitationId = citationId;

            InitializeComponent();

            StartPosition = FormStartPosition.Manual;
            Top = Y;
            Left = X;

            categoryUserControl1.CurrentCitationId = citationId;

            if (m_CategoryService == null)
                Mef.Compose(this);
        }

        private void Form_ReportCategory_Load(object sender, EventArgs e)
        {
            categoryUserControl1.Focus();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region Resize ================================================

        private bool mouseDown;
        private Point lastLocation;

        private void LabelRezise_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void LabelRezise_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown || !(sender is Label))
                return;

            Point delta = new Point(
                (this.Size.Width - lastLocation.X) + e.X,
                (this.Size.Height - lastLocation.Y) + e.Y);

            Width = delta.X;
            Height = delta.Y;

            this.Update();
        }

        private void LabelRezise_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        #endregion Resize =============================================
    }
}
