namespace Dek.Bel
{
    partial class FormAbout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label_version = new System.Windows.Forms.Label();
            this.label_BelWebsite = new System.Windows.Forms.Label();
            this.label_SumatraWebsite = new System.Windows.Forms.Label();
            this.label_Sumatra = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_license = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 30.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(360, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bel";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(12, 343);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(361, 51);
            this.button1.TabIndex = 1;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label_version
            // 
            this.label_version.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_version.BackColor = System.Drawing.Color.Transparent;
            this.label_version.Location = new System.Drawing.Point(12, 133);
            this.label_version.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(361, 29);
            this.label_version.TabIndex = 2;
            this.label_version.Text = "Version 0.0.0";
            this.label_version.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_BelWebsite
            // 
            this.label_BelWebsite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_BelWebsite.BackColor = System.Drawing.Color.Transparent;
            this.label_BelWebsite.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_BelWebsite.Location = new System.Drawing.Point(11, 162);
            this.label_BelWebsite.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_BelWebsite.Name = "label_BelWebsite";
            this.label_BelWebsite.Size = new System.Drawing.Size(361, 29);
            this.label_BelWebsite.TabIndex = 3;
            this.label_BelWebsite.Text = "https://github.com/draconculis/BelPdf";
            this.label_BelWebsite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_BelWebsite.Click += new System.EventHandler(this.label_BelWebsite_Click);
            this.label_BelWebsite.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.label_BelWebsite.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // label_SumatraWebsite
            // 
            this.label_SumatraWebsite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_SumatraWebsite.BackColor = System.Drawing.Color.Transparent;
            this.label_SumatraWebsite.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_SumatraWebsite.Location = new System.Drawing.Point(12, 295);
            this.label_SumatraWebsite.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_SumatraWebsite.Name = "label_SumatraWebsite";
            this.label_SumatraWebsite.Size = new System.Drawing.Size(361, 29);
            this.label_SumatraWebsite.TabIndex = 5;
            this.label_SumatraWebsite.Text = "https://github.com/sumatrapdfreader/sumatrapdf";
            this.label_SumatraWebsite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_SumatraWebsite.Click += new System.EventHandler(this.label_SumatraWebsite_Click);
            this.label_SumatraWebsite.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.label_SumatraWebsite.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // label_Sumatra
            // 
            this.label_Sumatra.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Sumatra.BackColor = System.Drawing.Color.Transparent;
            this.label_Sumatra.Location = new System.Drawing.Point(8, 266);
            this.label_Sumatra.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Sumatra.Name = "label_Sumatra";
            this.label_Sumatra.Size = new System.Drawing.Size(365, 29);
            this.label_Sumatra.TabIndex = 4;
            this.label_Sumatra.Text = "Based on Sumatra Pdf";
            this.label_Sumatra.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 191);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(361, 29);
            this.label2.TabIndex = 6;
            this.label2.Text = "Report an issue";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            this.label2.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.label2.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // label_license
            // 
            this.label_license.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_license.BackColor = System.Drawing.Color.Transparent;
            this.label_license.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_license.Location = new System.Drawing.Point(12, 223);
            this.label_license.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_license.Name = "label_license";
            this.label_license.Size = new System.Drawing.Size(361, 29);
            this.label_license.TabIndex = 7;
            this.label_license.Text = "License GPL v3";
            this.label_license.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_license.Click += new System.EventHandler(this.label_license_Click);
            this.label_license.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.label_license.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::Dek.Bel.Properties.Resources.bel;
            this.pictureBox1.Location = new System.Drawing.Point(155, 77);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(70, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(385, 403);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label_license);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_SumatraWebsite);
            this.Controls.Add(this.label_Sumatra);
            this.Controls.Add(this.label_BelWebsite);
            this.Controls.Add(this.label_version);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bel";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label_version;
        private System.Windows.Forms.Label label_BelWebsite;
        private System.Windows.Forms.Label label_SumatraWebsite;
        private System.Windows.Forms.Label label_Sumatra;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_license;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}