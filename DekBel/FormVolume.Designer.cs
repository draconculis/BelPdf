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
            this.listBox_books = new System.Windows.Forms.ListBox();
            this.listBox_chapters = new System.Windows.Forms.ListBox();
            this.listBox_subchapters = new System.Windows.Forms.ListBox();
            this.listBox_paragraphs = new System.Windows.Forms.ListBox();
            this.button_close = new System.Windows.Forms.Button();
            this.button_selectAndClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.listBox_CitationsWithReferences = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_title
            // 
            this.textBox_title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_title.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_title.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_title.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_title.Location = new System.Drawing.Point(3, 3);
            this.textBox_title.Multiline = true;
            this.textBox_title.Name = "textBox_title";
            this.textBox_title.Size = new System.Drawing.Size(1032, 42);
            this.textBox_title.TabIndex = 0;
            this.textBox_title.Text = "Book title";
            this.textBox_title.Leave += new System.EventHandler(this.TextBox_title_Leave);
            // 
            // listBox_books
            // 
            this.listBox_books.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_books.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox_books.FormattingEnabled = true;
            this.listBox_books.IntegralHeight = false;
            this.listBox_books.ItemHeight = 15;
            this.listBox_books.Location = new System.Drawing.Point(3, 27);
            this.listBox_books.Name = "listBox_books";
            this.listBox_books.Size = new System.Drawing.Size(252, 128);
            this.listBox_books.TabIndex = 0;
            this.listBox_books.SelectedIndexChanged += new System.EventHandler(this.ListBox_SelectedIndexChanged);
            // 
            // listBox_chapters
            // 
            this.listBox_chapters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_chapters.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox_chapters.FormattingEnabled = true;
            this.listBox_chapters.IntegralHeight = false;
            this.listBox_chapters.ItemHeight = 15;
            this.listBox_chapters.Location = new System.Drawing.Point(261, 27);
            this.listBox_chapters.Name = "listBox_chapters";
            this.listBox_chapters.Size = new System.Drawing.Size(252, 128);
            this.listBox_chapters.TabIndex = 1;
            this.listBox_chapters.SelectedIndexChanged += new System.EventHandler(this.ListBox_SelectedIndexChanged);
            // 
            // listBox_subchapters
            // 
            this.listBox_subchapters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_subchapters.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox_subchapters.FormattingEnabled = true;
            this.listBox_subchapters.IntegralHeight = false;
            this.listBox_subchapters.ItemHeight = 15;
            this.listBox_subchapters.Location = new System.Drawing.Point(519, 27);
            this.listBox_subchapters.Name = "listBox_subchapters";
            this.listBox_subchapters.Size = new System.Drawing.Size(252, 128);
            this.listBox_subchapters.TabIndex = 1;
            this.listBox_subchapters.SelectedIndexChanged += new System.EventHandler(this.ListBox_SelectedIndexChanged);
            // 
            // listBox_paragraphs
            // 
            this.listBox_paragraphs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_paragraphs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox_paragraphs.FormattingEnabled = true;
            this.listBox_paragraphs.IntegralHeight = false;
            this.listBox_paragraphs.ItemHeight = 15;
            this.listBox_paragraphs.Location = new System.Drawing.Point(777, 27);
            this.listBox_paragraphs.Name = "listBox_paragraphs";
            this.listBox_paragraphs.Size = new System.Drawing.Size(252, 128);
            this.listBox_paragraphs.TabIndex = 1;
            this.listBox_paragraphs.SelectedIndexChanged += new System.EventHandler(this.ListBox_SelectedIndexChanged);
            // 
            // button_close
            // 
            this.button_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_close.Location = new System.Drawing.Point(850, 346);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(177, 26);
            this.button_close.TabIndex = 3;
            this.button_close.Text = "Close";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button_selectAndClose
            // 
            this.button_selectAndClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_selectAndClose.Location = new System.Drawing.Point(667, 346);
            this.button_selectAndClose.Name = "button_selectAndClose";
            this.button_selectAndClose.Size = new System.Drawing.Size(177, 27);
            this.button_selectAndClose.TabIndex = 4;
            this.button_selectAndClose.Text = "Select citation and close";
            this.button_selectAndClose.UseVisualStyleBackColor = true;
            this.button_selectAndClose.Click += new System.EventHandler(this.Button_selectAndClose_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.listBox_CitationsWithReferences, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.listBox_books, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.listBox_paragraphs, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBox_chapters, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBox_subchapters, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 47);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1032, 293);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // listBox_CitationsWithReferences
            // 
            this.listBox_CitationsWithReferences.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_CitationsWithReferences.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel1.SetColumnSpan(this.listBox_CitationsWithReferences, 4);
            this.listBox_CitationsWithReferences.FormattingEnabled = true;
            this.listBox_CitationsWithReferences.IntegralHeight = false;
            this.listBox_CitationsWithReferences.ItemHeight = 15;
            this.listBox_CitationsWithReferences.Location = new System.Drawing.Point(3, 161);
            this.listBox_CitationsWithReferences.Name = "listBox_CitationsWithReferences";
            this.listBox_CitationsWithReferences.Size = new System.Drawing.Size(1026, 129);
            this.listBox_CitationsWithReferences.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(777, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(252, 24);
            this.label4.TabIndex = 8;
            this.label4.Text = "Paragraphs";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(519, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(252, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "Subchapters";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(252, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Books";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(261, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(252, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "Chapters";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormVolume
            // 
            this.AcceptButton = this.button_selectAndClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_close;
            this.ClientSize = new System.Drawing.Size(1038, 380);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button_selectAndClose);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.textBox_title);
            this.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormVolume";
            this.Text = "Volume";
            this.Load += new System.EventHandler(this.FormVolume_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_title;
        private System.Windows.Forms.ListBox listBox_books;
        private System.Windows.Forms.ListBox listBox_chapters;
        private System.Windows.Forms.ListBox listBox_subchapters;
        private System.Windows.Forms.ListBox listBox_paragraphs;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Button button_selectAndClose;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox listBox_CitationsWithReferences;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}