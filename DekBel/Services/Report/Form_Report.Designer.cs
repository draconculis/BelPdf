namespace Dek.Bel.Services.Report
{
    partial class Form_Report
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Report));
            this.Columns = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_columns = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.label_time = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button_ShowHtml = new System.Windows.Forms.Button();
            this.sfDataGrid1 = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.buttonShowFilter = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button_showReportsFolder = new System.Windows.Forms.Button();
            this.button_ExportCsv = new System.Windows.Forms.Button();
            this.button_ExportHtml = new System.Windows.Forms.Button();
            this.button_ExportPdf = new System.Windows.Forms.Button();
            this.button_ExportExcel = new System.Windows.Forms.Button();
            this.button_SelectAndClose = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button_expandGroups = new System.Windows.Forms.Button();
            this.button_resetGroups = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyCellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setCategoryWeightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.setCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.Columns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Columns
            // 
            this.Columns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_columns});
            this.Columns.Name = "contextMenuStrip1";
            this.Columns.Size = new System.Drawing.Size(132, 26);
            this.Columns.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripMenuItem_columns
            // 
            this.toolStripMenuItem_columns.Name = "toolStripMenuItem_columns";
            this.toolStripMenuItem_columns.Size = new System.Drawing.Size(131, 22);
            this.toolStripMenuItem_columns.Text = "Columns...";
            this.toolStripMenuItem_columns.Click += new System.EventHandler(this.ToolStripMenuItem_columns_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1053, 446);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(182, 27);
            this.button1.TabIndex = 1;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // label_time
            // 
            this.label_time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_time.AutoSize = true;
            this.label_time.Location = new System.Drawing.Point(4, 467);
            this.label_time.Name = "label_time";
            this.label_time.Size = new System.Drawing.Size(34, 14);
            this.label_time.TabIndex = 2;
            this.label_time.Text = "Took:";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(211, 320);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(91, 29);
            this.button3.TabIndex = 7;
            this.button3.Text = "Columns...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.Location = new System.Drawing.Point(308, 320);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(91, 29);
            this.button4.TabIndex = 8;
            this.button4.Text = "Show all cols";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // button_ShowHtml
            // 
            this.button_ShowHtml.Location = new System.Drawing.Point(107, 49);
            this.button_ShowHtml.Name = "button_ShowHtml";
            this.button_ShowHtml.Size = new System.Drawing.Size(188, 25);
            this.button_ShowHtml.TabIndex = 10;
            this.button_ShowHtml.Text = "HTML (Show in browser)";
            this.button_ShowHtml.UseVisualStyleBackColor = true;
            this.button_ShowHtml.Click += new System.EventHandler(this.Button5_Click);
            // 
            // sfDataGrid1
            // 
            this.sfDataGrid1.AccessibleName = "Table";
            this.sfDataGrid1.AllowDraggingColumns = true;
            this.sfDataGrid1.AllowFiltering = true;
            this.sfDataGrid1.AllowResizingColumns = true;
            this.sfDataGrid1.AllowTriStateSorting = true;
            this.sfDataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sfDataGrid1.FilterRowPosition = Syncfusion.WinForms.DataGrid.Enums.RowPosition.Top;
            this.sfDataGrid1.Font = new System.Drawing.Font("Corbel", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sfDataGrid1.HeaderRowHeight = 28;
            this.sfDataGrid1.Location = new System.Drawing.Point(1, 2);
            this.sfDataGrid1.Name = "sfDataGrid1";
            this.sfDataGrid1.PasteOption = Syncfusion.WinForms.DataGrid.Enums.PasteOptions.None;
            this.sfDataGrid1.ShowGroupDropArea = true;
            this.sfDataGrid1.ShowRowHeaderErrorIcon = false;
            this.sfDataGrid1.Size = new System.Drawing.Size(1239, 311);
            this.sfDataGrid1.TabIndex = 11;
            this.sfDataGrid1.Text = "sfDataGrid1";
            this.sfDataGrid1.SelectionChanged += new Syncfusion.WinForms.DataGrid.Events.SelectionChangedEventHandler(this.sfDataGrid1_SelectionChanged);
            this.sfDataGrid1.CurrentCellBeginEdit += new Syncfusion.WinForms.DataGrid.Events.CurrentCellBeginEditEventHandler(this.sfDataGrid1_CurrentCellBeginEdit);
            // 
            // buttonShowFilter
            // 
            this.buttonShowFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonShowFilter.Location = new System.Drawing.Point(12, 320);
            this.buttonShowFilter.Name = "buttonShowFilter";
            this.buttonShowFilter.Size = new System.Drawing.Size(96, 29);
            this.buttonShowFilter.TabIndex = 12;
            this.buttonShowFilter.Text = "Toggle filter";
            this.buttonShowFilter.UseVisualStyleBackColor = true;
            this.buttonShowFilter.Click += new System.EventHandler(this.buttonShowFilter_Click);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button6.Location = new System.Drawing.Point(114, 320);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(91, 29);
            this.button6.TabIndex = 13;
            this.button6.Text = "Clear filter";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button_showReportsFolder);
            this.groupBox1.Controls.Add(this.button_ExportCsv);
            this.groupBox1.Controls.Add(this.button_ExportHtml);
            this.groupBox1.Controls.Add(this.button_ExportPdf);
            this.groupBox1.Controls.Add(this.button_ExportExcel);
            this.groupBox1.Controls.Add(this.button_ShowHtml);
            this.groupBox1.Location = new System.Drawing.Point(7, 360);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1233, 78);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(1046, 49);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(182, 25);
            this.button2.TabIndex = 16;
            this.button2.Text = "Show latest reports folder...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button_showReportsFolder
            // 
            this.button_showReportsFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_showReportsFolder.Location = new System.Drawing.Point(1046, 18);
            this.button_showReportsFolder.Name = "button_showReportsFolder";
            this.button_showReportsFolder.Size = new System.Drawing.Size(182, 25);
            this.button_showReportsFolder.TabIndex = 15;
            this.button_showReportsFolder.Text = "Show default reports folder...";
            this.button_showReportsFolder.UseVisualStyleBackColor = true;
            this.button_showReportsFolder.Click += new System.EventHandler(this.button_showReportsFolder_Click);
            // 
            // button_ExportCsv
            // 
            this.button_ExportCsv.Location = new System.Drawing.Point(107, 18);
            this.button_ExportCsv.Name = "button_ExportCsv";
            this.button_ExportCsv.Size = new System.Drawing.Size(91, 25);
            this.button_ExportCsv.TabIndex = 14;
            this.button_ExportCsv.Text = "CSV...";
            this.button_ExportCsv.UseVisualStyleBackColor = true;
            this.button_ExportCsv.Click += new System.EventHandler(this.button_ExportCsv_Click);
            // 
            // button_ExportHtml
            // 
            this.button_ExportHtml.Location = new System.Drawing.Point(10, 49);
            this.button_ExportHtml.Name = "button_ExportHtml";
            this.button_ExportHtml.Size = new System.Drawing.Size(91, 25);
            this.button_ExportHtml.TabIndex = 13;
            this.button_ExportHtml.Text = "Html...";
            this.button_ExportHtml.UseVisualStyleBackColor = true;
            this.button_ExportHtml.Click += new System.EventHandler(this.button_ExportHtml_Click);
            // 
            // button_ExportPdf
            // 
            this.button_ExportPdf.Location = new System.Drawing.Point(204, 18);
            this.button_ExportPdf.Name = "button_ExportPdf";
            this.button_ExportPdf.Size = new System.Drawing.Size(91, 25);
            this.button_ExportPdf.TabIndex = 12;
            this.button_ExportPdf.Text = "Pdf...";
            this.button_ExportPdf.UseVisualStyleBackColor = true;
            this.button_ExportPdf.Click += new System.EventHandler(this.button_ExportPdf_Click);
            // 
            // button_ExportExcel
            // 
            this.button_ExportExcel.Location = new System.Drawing.Point(10, 18);
            this.button_ExportExcel.Name = "button_ExportExcel";
            this.button_ExportExcel.Size = new System.Drawing.Size(91, 25);
            this.button_ExportExcel.TabIndex = 11;
            this.button_ExportExcel.Text = "Excel...";
            this.button_ExportExcel.UseVisualStyleBackColor = true;
            this.button_ExportExcel.Click += new System.EventHandler(this.button_ExportExcel_Click);
            // 
            // button_SelectAndClose
            // 
            this.button_SelectAndClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SelectAndClose.Location = new System.Drawing.Point(866, 446);
            this.button_SelectAndClose.Name = "button_SelectAndClose";
            this.button_SelectAndClose.Size = new System.Drawing.Size(181, 27);
            this.button_SelectAndClose.TabIndex = 16;
            this.button_SelectAndClose.Text = "Select and Close";
            this.button_SelectAndClose.UseVisualStyleBackColor = true;
            this.button_SelectAndClose.Click += new System.EventHandler(this.button_SelectAndClose_Click);
            // 
            // button_expandGroups
            // 
            this.button_expandGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_expandGroups.Location = new System.Drawing.Point(405, 320);
            this.button_expandGroups.Name = "button_expandGroups";
            this.button_expandGroups.Size = new System.Drawing.Size(91, 29);
            this.button_expandGroups.TabIndex = 17;
            this.button_expandGroups.Text = "Expand groups";
            this.button_expandGroups.UseVisualStyleBackColor = true;
            this.button_expandGroups.Click += new System.EventHandler(this.button_expandGroups_Click);
            // 
            // button_resetGroups
            // 
            this.button_resetGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_resetGroups.Location = new System.Drawing.Point(502, 320);
            this.button_resetGroups.Name = "button_resetGroups";
            this.button_resetGroups.Size = new System.Drawing.Size(91, 29);
            this.button_resetGroups.TabIndex = 18;
            this.button_resetGroups.Text = "Reset groups";
            this.button_resetGroups.UseVisualStyleBackColor = true;
            this.button_resetGroups.Click += new System.EventHandler(this.button_resetGroups_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyCellToolStripMenuItem,
            this.copyRowToolStripMenuItem,
            this.setCategoryWeightToolStripMenuItem,
            this.setCategoryToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(179, 92);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // copyCellToolStripMenuItem
            // 
            this.copyCellToolStripMenuItem.Name = "copyCellToolStripMenuItem";
            this.copyCellToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.copyCellToolStripMenuItem.Text = "Copy Cell";
            // 
            // copyRowToolStripMenuItem
            // 
            this.copyRowToolStripMenuItem.Name = "copyRowToolStripMenuItem";
            this.copyRowToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.copyRowToolStripMenuItem.Text = "Copy row";
            // 
            // setCategoryWeightToolStripMenuItem
            // 
            this.setCategoryWeightToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.setCategoryWeightToolStripMenuItem.Name = "setCategoryWeightToolStripMenuItem";
            this.setCategoryWeightToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.setCategoryWeightToolStripMenuItem.Text = "Category weight";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(80, 22);
            this.toolStripMenuItem2.Text = "1";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(80, 22);
            this.toolStripMenuItem3.Text = "2";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(80, 22);
            this.toolStripMenuItem4.Text = "3";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(80, 22);
            this.toolStripMenuItem5.Text = "4";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(80, 22);
            this.toolStripMenuItem6.Text = "5";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // setCategoryToolStripMenuItem
            // 
            this.setCategoryToolStripMenuItem.Name = "setCategoryToolStripMenuItem";
            this.setCategoryToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.setCategoryToolStripMenuItem.Text = "Set main category...";
            this.setCategoryToolStripMenuItem.Click += new System.EventHandler(this.setCategoryToolStripMenuItem_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox1.Location = new System.Drawing.Point(1053, 320);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(182, 29);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Only show current Volume";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Form_Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 485);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button_resetGroups);
            this.Controls.Add(this.button_expandGroups);
            this.Controls.Add(this.button_SelectAndClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.buttonShowFilter);
            this.Controls.Add(this.sfDataGrid1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label_time);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Corbel", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_Report";
            this.Text = "Report";
            this.Load += new System.EventHandler(this.Form_Report_Load);
            this.Columns.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip Columns;
        private System.Windows.Forms.Label label_time;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_columns;
        private System.Windows.Forms.Button button_ShowHtml;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid1;
        private System.Windows.Forms.Button buttonShowFilter;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_ExportCsv;
        private System.Windows.Forms.Button button_ExportHtml;
        private System.Windows.Forms.Button button_ExportPdf;
        private System.Windows.Forms.Button button_ExportExcel;
        private System.Windows.Forms.Button button_SelectAndClose;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button_showReportsFolder;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button_expandGroups;
        private System.Windows.Forms.Button button_resetGroups;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyCellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyRowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setCategoryWeightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem setCategoryToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}