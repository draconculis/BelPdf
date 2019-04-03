namespace Dek.Bel
{
    partial class FormVolume
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVolume));
            this.textBox_title = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_books = new System.Windows.Forms.TabPage();
            this.tabPage_chapters = new System.Windows.Forms.TabPage();
            this.tabPage_subChapters = new System.Windows.Forms.TabPage();
            this.tabPage_paragraphs = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabPage_citations = new System.Windows.Forms.TabPage();
            this.listBox_books = new System.Windows.Forms.ListBox();
            this.listBox_chapters = new System.Windows.Forms.ListBox();
            this.listBox_subchapters = new System.Windows.Forms.ListBox();
            this.listBox_paragraphs = new System.Windows.Forms.ListBox();
            this.listBox_citations = new System.Windows.Forms.ListBox();
            this.button_close = new System.Windows.Forms.Button();
            this.button_selectAndClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage_books.SuspendLayout();
            this.tabPage_chapters.SuspendLayout();
            this.tabPage_subChapters.SuspendLayout();
            this.tabPage_paragraphs.SuspendLayout();
            this.tabPage_citations.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_title
            // 
            this.textBox_title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_title.Location = new System.Drawing.Point(3, 3);
            this.textBox_title.Multiline = true;
            this.textBox_title.Name = "textBox_title";
            this.textBox_title.Size = new System.Drawing.Size(927, 42);
            this.textBox_title.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Controls.Add(this.tabPage_books);
            this.tabControl1.Controls.Add(this.tabPage_chapters);
            this.tabControl1.Controls.Add(this.tabPage_subChapters);
            this.tabControl1.Controls.Add(this.tabPage_paragraphs);
            this.tabControl1.Controls.Add(this.tabPage_citations);
            this.tabControl1.Location = new System.Drawing.Point(3, 130);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(927, 267);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_books
            // 
            this.tabPage_books.Controls.Add(this.listBox_books);
            this.tabPage_books.Location = new System.Drawing.Point(4, 24);
            this.tabPage_books.Name = "tabPage_books";
            this.tabPage_books.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_books.Size = new System.Drawing.Size(919, 239);
            this.tabPage_books.TabIndex = 0;
            this.tabPage_books.Text = "Books";
            this.tabPage_books.UseVisualStyleBackColor = true;
            // 
            // tabPage_chapters
            // 
            this.tabPage_chapters.Controls.Add(this.listBox_chapters);
            this.tabPage_chapters.Location = new System.Drawing.Point(4, 24);
            this.tabPage_chapters.Name = "tabPage_chapters";
            this.tabPage_chapters.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_chapters.Size = new System.Drawing.Size(919, 239);
            this.tabPage_chapters.TabIndex = 1;
            this.tabPage_chapters.Text = "Chapters";
            this.tabPage_chapters.UseVisualStyleBackColor = true;
            // 
            // tabPage_subChapters
            // 
            this.tabPage_subChapters.Controls.Add(this.listBox_subchapters);
            this.tabPage_subChapters.Location = new System.Drawing.Point(4, 24);
            this.tabPage_subChapters.Name = "tabPage_subChapters";
            this.tabPage_subChapters.Size = new System.Drawing.Size(919, 239);
            this.tabPage_subChapters.TabIndex = 2;
            this.tabPage_subChapters.Text = "Sub chapters";
            this.tabPage_subChapters.UseVisualStyleBackColor = true;
            // 
            // tabPage_paragraphs
            // 
            this.tabPage_paragraphs.Controls.Add(this.listBox_paragraphs);
            this.tabPage_paragraphs.Location = new System.Drawing.Point(4, 24);
            this.tabPage_paragraphs.Name = "tabPage_paragraphs";
            this.tabPage_paragraphs.Size = new System.Drawing.Size(919, 239);
            this.tabPage_paragraphs.TabIndex = 3;
            this.tabPage_paragraphs.Text = "Paragraphs";
            this.tabPage_paragraphs.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(3, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(923, 73);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Currently selected";
            // 
            // tabPage_citations
            // 
            this.tabPage_citations.Controls.Add(this.listBox_citations);
            this.tabPage_citations.Location = new System.Drawing.Point(4, 24);
            this.tabPage_citations.Name = "tabPage_citations";
            this.tabPage_citations.Size = new System.Drawing.Size(919, 239);
            this.tabPage_citations.TabIndex = 4;
            this.tabPage_citations.Text = "Citations";
            this.tabPage_citations.UseVisualStyleBackColor = true;
            // 
            // listBox_books
            // 
            this.listBox_books.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox_books.FormattingEnabled = true;
            this.listBox_books.ItemHeight = 15;
            this.listBox_books.Location = new System.Drawing.Point(6, 5);
            this.listBox_books.Name = "listBox_books";
            this.listBox_books.Size = new System.Drawing.Size(336, 229);
            this.listBox_books.TabIndex = 0;
            // 
            // listBox_chapters
            // 
            this.listBox_chapters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox_chapters.FormattingEnabled = true;
            this.listBox_chapters.ItemHeight = 15;
            this.listBox_chapters.Location = new System.Drawing.Point(6, 4);
            this.listBox_chapters.Name = "listBox_chapters";
            this.listBox_chapters.Size = new System.Drawing.Size(336, 229);
            this.listBox_chapters.TabIndex = 1;
            // 
            // listBox_subchapters
            // 
            this.listBox_subchapters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox_subchapters.FormattingEnabled = true;
            this.listBox_subchapters.ItemHeight = 15;
            this.listBox_subchapters.Location = new System.Drawing.Point(5, 3);
            this.listBox_subchapters.Name = "listBox_subchapters";
            this.listBox_subchapters.Size = new System.Drawing.Size(336, 229);
            this.listBox_subchapters.TabIndex = 1;
            // 
            // listBox_paragraphs
            // 
            this.listBox_paragraphs.FormattingEnabled = true;
            this.listBox_paragraphs.ItemHeight = 15;
            this.listBox_paragraphs.Location = new System.Drawing.Point(5, 3);
            this.listBox_paragraphs.Name = "listBox_paragraphs";
            this.listBox_paragraphs.Size = new System.Drawing.Size(336, 229);
            this.listBox_paragraphs.TabIndex = 1;
            // 
            // listBox_citations
            // 
            this.listBox_citations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_citations.FormattingEnabled = true;
            this.listBox_citations.ItemHeight = 15;
            this.listBox_citations.Location = new System.Drawing.Point(3, 3);
            this.listBox_citations.Name = "listBox_citations";
            this.listBox_citations.Size = new System.Drawing.Size(911, 229);
            this.listBox_citations.TabIndex = 1;
            // 
            // button_close
            // 
            this.button_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_close.Location = new System.Drawing.Point(782, 399);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(144, 27);
            this.button_close.TabIndex = 3;
            this.button_close.Text = "Close";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button_selectAndClose
            // 
            this.button_selectAndClose.Location = new System.Drawing.Point(599, 399);
            this.button_selectAndClose.Name = "button_selectAndClose";
            this.button_selectAndClose.Size = new System.Drawing.Size(177, 27);
            this.button_selectAndClose.TabIndex = 4;
            this.button_selectAndClose.Text = "Select citation and close";
            this.button_selectAndClose.UseVisualStyleBackColor = true;
            this.button_selectAndClose.Click += new System.EventHandler(this.Button_selectAndClose_Click);
            // 
            // FormVolume
            // 
            this.AcceptButton = this.button_selectAndClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_close;
            this.ClientSize = new System.Drawing.Size(933, 429);
            this.Controls.Add(this.button_selectAndClose);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.textBox_title);
            this.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormVolume";
            this.Text = "Volume";
            this.Load += new System.EventHandler(this.FormVolume_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_books.ResumeLayout(false);
            this.tabPage_chapters.ResumeLayout(false);
            this.tabPage_subChapters.ResumeLayout(false);
            this.tabPage_paragraphs.ResumeLayout(false);
            this.tabPage_citations.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_title;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_books;
        private System.Windows.Forms.TabPage tabPage_chapters;
        private System.Windows.Forms.TabPage tabPage_subChapters;
        private System.Windows.Forms.TabPage tabPage_paragraphs;
        private System.Windows.Forms.TabPage tabPage_citations;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox_books;
        private System.Windows.Forms.ListBox listBox_chapters;
        private System.Windows.Forms.ListBox listBox_subchapters;
        private System.Windows.Forms.ListBox listBox_paragraphs;
        private System.Windows.Forms.ListBox listBox_citations;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Button button_selectAndClose;
    }
}