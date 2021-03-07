
namespace Dek.Bel.Services.Report
{
    partial class Form_ReportCategory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_OK = new System.Windows.Forms.Button();
            this.label_resize = new System.Windows.Forms.Label();
            this.categoryUserControl1 = new Dek.Bel.Services.Categories.CategoryUserControl();
            this.SuspendLayout();
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.Location = new System.Drawing.Point(235, 235);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(119, 24);
            this.button_OK.TabIndex = 4;
            this.button_OK.Text = "Done";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            this.button_OK.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LabelRezise_MouseUp);
            // 
            // label_resize
            // 
            this.label_resize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label_resize.Location = new System.Drawing.Point(347, 252);
            this.label_resize.Name = "label_resize";
            this.label_resize.Size = new System.Drawing.Size(14, 17);
            this.label_resize.TabIndex = 5;
            this.label_resize.Text = ".:";
            this.label_resize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LabelRezise_MouseDown);
            this.label_resize.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LabelRezise_MouseMove);
            this.label_resize.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LabelRezise_MouseUp);
            // 
            // categoryUserControl1
            // 
            this.categoryUserControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryUserControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.categoryUserControl1.Font = new System.Drawing.Font("Corbel", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categoryUserControl1.Location = new System.Drawing.Point(5, 5);
            this.categoryUserControl1.m_CategoryLabelService = null;
            this.categoryUserControl1.m_CategoryService = null;
            this.categoryUserControl1.m_DBService = null;
            this.categoryUserControl1.Name = "categoryUserControl1";
            this.categoryUserControl1.Size = new System.Drawing.Size(351, 228);
            this.categoryUserControl1.TabIndex = 6;
            // 
            // Form_ReportCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 268);
            this.Controls.Add(this.categoryUserControl1);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.label_resize);
            this.Font = new System.Drawing.Font("Corbel", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_ReportCategory";
            this.Text = "Form_ReportCategory";
            this.Load += new System.EventHandler(this.Form_ReportCategory_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LabelRezise_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Label label_resize;
        private Categories.CategoryUserControl categoryUserControl1;
    }
}