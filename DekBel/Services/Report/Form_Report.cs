using Dek.Cls;
using Dek.Bel.Core.GUI;
using Dek.Bel.Core.Services;
using Dek.Cls.SyncFusion;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dek.Bel.Services.Report
{
    public partial class Form_Report : Form
    {
        [Import] public ReportService m_ReportService { get; set; }
        [Import] public IMessageboxService m_MessageBoxService { get; set; }
        [Import] public IUserSettingsService m_UserSettingsService { get; set; }

        //private List<string> AlwaysHiddenColumns = new List<string> 
        //{
        //    nameof(ReportModel.Emphasis)
        //};

        public readonly string DefaultExportSaveFolder;
        string ExportSaveFolder = string.Empty;

        private List<string> DefaultHiddenColumns = new List<string>
        {
        };


        public Form_Report()
        {
            InitializeComponent();
            if (m_ReportService == null)
                Mef.Compose(this);

            string ExportDefaultSaveFolder = Path.Combine(Environment.SpecialFolder.MyDocuments.ToString(), "BelPdf");

            sfDataGrid1.RecordContextMenu = new ContextMenuStrip();
            sfDataGrid1.RecordContextMenu.Items.Add("Copy", null, OnCopyClicked);

            GenerateData();
            SetDataSource();
        }




        private void GenerateData()
        {
            long milliseconds = 0;
            try
            {
                milliseconds = m_ReportService.GenerateReport();
            }
            catch (Exception aex)
            {
                m_MessageBoxService.Show($"Exception: {aex.Message}", "Exception when generating report");
            }
            label_time.Text = $"Took: {milliseconds / 1000.0} seconds";
        }

        private void SetDataSource()
        {
            var source = new BindingSource();
            source.DataSource = m_ReportService.Filtered;

            sfDataGrid1.DataSource = source;

            sfDataGrid1.Columns[nameof(ReportModel.Emphasis)].Visible = false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Predicate<ReportModel> filter = x => x.MainCategory.StartsWith(textBox1.Text);

            m_ReportService.Filter = filter;
            SetDataSource();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            ShowColumnSelector();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            foreach (var col in sfDataGrid1.Columns)
            {
                col.Visible = true;
            }
        }

        private void ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void ToolStripMenuItem_columns_Click(object sender, EventArgs e)
        {
            ShowColumnSelector();
        }

        private void ShowColumnSelector()
        {
            Form_ReportColumns formCols = new Form_ReportColumns(sfDataGrid1);
            formCols.ShowDialog();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            string title = $"Bel Report {DateTime.Now.ToSaneStringShort()}";
            string fileName = $"Bel_Report_{DateTime.Now.ToCompactStringShort()}.html";

            StringBuilder sb = new StringBuilder();

            sb.Append($"<html><head><title>{title}</title>");
            AppendReportTableStyle(sb);
            sb.Append("</head><body>");
            // Page title
            sb.Append($"<h1>{title}</h1>");
            sb.Append(Environment.NewLine);
            sb.Append("<table id=\"report\">");
            // Headers
            sb.Append("<tr>");
            sb.Append(Environment.NewLine);
            foreach (GridColumn col in sfDataGrid1.Columns)
            {
                if (!col.Visible)
                    continue;

                sb.Append("<th>");
                sb.Append(col.HeaderText);
                sb.Append("</th>" + Environment.NewLine);
            }
            sb.Append("</tr>");
            sb.Append(Environment.NewLine);



            //foreach (var record in sfDataGrid1.View.Records)
            //{
            //    record.
            //}

            // foreach (RecordEntry row in sfDataGrid1.View.Records.Where(x => x.IsRecords))

            int rowcount = sfDataGrid1.RowCount;
            for (int rowIdx = 0; rowIdx < sfDataGrid1.View.Records.Where(x => x.IsRecords).Count(); rowIdx++)
            {
                // No need for this as View doesn't have it
                //if (sfDataGrid1.IsFilterRowIndex(rowIdx))
                //    continue;

                //object record = sfDataGrid1.GetRecordAtRowIndex(rowIdx);

                if (sfDataGrid1.View.Filter != null && !sfDataGrid1.View.Filter(sfDataGrid1.GetRecordAtRowIndex(rowIdx)))
                    continue;

                sb.Append("<tr>");
                sb.Append(Environment.NewLine);

                foreach (GridColumn col in sfDataGrid1.Columns)
                {
                    if (!col.Visible)
                        continue;

                    sb.Append("<td>");

                    string cellValue = GetCellValue(rowIdx, col.MappingName);

                    if (IsCitation(col))
                    {
                        string emphasis = GetCellValue(rowIdx, nameof(ReportModel.Emphasis));
                        HandleCitation3(sb, cellValue, emphasis); // Builds export html for citation
                    }
                    else
                        sb.Append(cellValue);

                    sb.Append("</td>" + Environment.NewLine);
                }
                sb.Append("</tr>");
                sb.Append(Environment.NewLine);
            }

            sb.Append("</table></body></html>");

            string reportPath = Path.Combine(m_UserSettingsService.StorageFolder, "Reports");
            if (!Directory.Exists(reportPath))
                Directory.CreateDirectory(reportPath);

            string filePath = Path.Combine(reportPath, fileName);
            File.WriteAllText(filePath, sb.ToString());

            System.Diagnostics.Process.Start(filePath);
        }

        private bool IsCitation(GridColumn col)
        {
            return col.HeaderText == nameof(ReportModel.Citation);
        }

        private void HandleCitation3(StringBuilder sb, string text, string emphasisString)
        {
            if (text.IsNullOrWhiteSpace())
                return;

            string s = text.Replace("\r\n", "\r").Replace("\n", "\r");

            List<DekRange> emphasis = new List<DekRange>();
            emphasis.LoadFromText(emphasisString);

            if (emphasis == null)
                emphasis = new List<DekRange>();

            bool boldEmphasis = m_UserSettingsService.BoldEmphasis;
            bool underlineEmphasis = m_UserSettingsService.UnderlineEmphasis;

            StringBuilder htmlStringBuilder = new StringBuilder();
            //rtfbuilder.Append(@"{\rtf1\ansi ");
            bool inEmphasis = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (emphasis.ContainsInteger(i) && !inEmphasis)
                {
                    if (boldEmphasis)
                        htmlStringBuilder.Append("<b>");
                    if (underlineEmphasis)
                        htmlStringBuilder.Append("<i>");
                    inEmphasis = true;
                }
                if (!emphasis.ContainsInteger(i) && inEmphasis)
                {
                    if (underlineEmphasis)
                        htmlStringBuilder.Append("</i>");
                    if (boldEmphasis)
                        htmlStringBuilder.Append("</b>");

                    inEmphasis = false;
                }

                htmlStringBuilder.Append(s[i]);
            }

            var ret = htmlStringBuilder.ToString();
            ret = ret.Replace("\r", "\r\n");

            sb.Append(ret);
        }

        private void AppendReportTableStyle(StringBuilder sb)
        {
            sb.Append(
                @"<style>
                    #report {
                    font-family: ""Trebuchet MS"", Arial, Helvetica, sans-serif;
                    border-collapse: collapse;
                    width: 200%;
                }

                #report td, #report th {
                    border: 1px solid #ddd;
                    padding: 8px;
                    vertical-align: top;
                }

                #report tr:nth-child(even){background-color: #f2f2f2;}

                #report tr:hover {background-color: #ffe;}

                #report th {
                    padding-top: 12px;
                    padding-bottom: 12px;
                    text-align: left;
                    background-color: #99ffcc;
                    color: Black;
                }
                </style>"
                );
        }

        private void buttonShowFilter_Click(object sender, EventArgs e)
        {
            sfDataGrid1.FilterRowPosition = (sfDataGrid1.FilterRowPosition == Syncfusion.WinForms.DataGrid.Enums.RowPosition.None)
                ? Syncfusion.WinForms.DataGrid.Enums.RowPosition.Top
                : Syncfusion.WinForms.DataGrid.Enums.RowPosition.None;
        }


        /// <summary>
        /// Context menu
        /// </summary>
        private void OnCopyClicked(object sender, EventArgs e)
        {
            sfDataGrid1.ClipboardController.Copy();
        }


        /*****************************************************/
        /** Some example code ********************************/
        /*****************************************************/

        // From https://help.syncfusion.com/windowsforms/datagrid/selection
        // Actually, wtf!
        private string GetTheValueOfACell(int row, int col)
        {
            // Get the cell value for RowIndex = 5 and ColumnIndex = 2
            string cellValue;
            int rowIndex = row;
            int columnIndex = sfDataGrid1.TableControl.ResolveToGridVisibleColumnIndex(col);
            if (columnIndex < 0)
                return string.Empty;

            var mappingName = sfDataGrid1.Columns[columnIndex].MappingName;
            var recordIndex = sfDataGrid1.TableControl.ResolveToRecordIndex(rowIndex);
            if (recordIndex < 0)
                return string.Empty;

            if (sfDataGrid1.View.TopLevelGroup != null)
            {
                var record = sfDataGrid1.View.TopLevelGroup.DisplayElements[recordIndex];
                if (!record.IsRecords)
                    return string.Empty;
                var data = (record as RecordEntry).Data;
                cellValue = (data.GetType().GetProperty(mappingName).GetValue(data, null).ToString());
            }
            else
            {
                var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                cellValue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
            }

            //MessageBox.Show(cellValue, "Value in cell (" + rowIndex + ", " + columnIndex + ")");
            return cellValue;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sfDataGrid1.ClearFilters();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ColumnChooserPopup columnChooser = new ColumnChooserPopup(this.sfDataGrid1);
            columnChooser.Show();
        }

        /*
        Getting a clicked cell's content

        this.sfDataGrid.CellClick += OnSfDataGridCellClick;   

        private void OnSfDataGridCellClick(object sender, CellClickEventArgs e)
        {
            // Get the row index value        
            var rowIndex = e.DataRow.RowIndex;
            //Get the column index value
            var columnIndex = e.DataColumn.ColumnIndex;
            //Get the cell value            
            var cellValue = this.sfDataGrid.View.GetPropertyAccessProvider().GetValue(e.DataRow.RowData, e.DataColumn.GridColumn.MappingName);
            MessageBox.Show("Cell Value \t:    " + cellValue + "\n" + "Row Index \t:    " + rowIndex + "\n" + "Column Index \t:    " + columnIndex, "Cell Value");
        }
        */



        private string GetCellValue(int rowIdx, int colIdx) => SfDataGridHelper.GetCellValue(sfDataGrid1, rowIdx, colIdx);
        private string GetCellValue(int rowIdx, string colName) => SfDataGridHelper.GetCellValue(sfDataGrid1, rowIdx, colName);

        private void sfDataGrid1_CurrentCellBeginEdit(object sender, Syncfusion.WinForms.DataGrid.Events.CurrentCellBeginEditEventArgs e)
        {
            if (e.DataRow.RowType != RowType.FilterRow)
                e.Cancel = true;
        }

        private void button_ExportExcel_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Excel files|*.xls;*.xlsx|All files|*.*";

            ShowSaveDialog("Export Excel");
        }

        private void button_ExportCsv_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt|All files|*.*";

            ShowSaveDialog("Export Csv");
        }

        private void button_ExportPdf_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Pdf files|*.pdf|All files|*.*";

            ShowSaveDialog("Export Pdf");
        }

        private void button_ExportHtml_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Html files|*.html;*.htm|All files|*.*";

            ShowSaveDialog("Export Html");
        }

        private void ShowSaveDialog(string header)
        {
            saveFileDialog1.InitialDirectory = string.IsNullOrWhiteSpace(ExportSaveFolder) ? DefaultExportSaveFolder : ExportSaveFolder;
            saveFileDialog1.Title = header;

            var result = saveFileDialog1.ShowDialog(this);
            if (result == DialogResult.Cancel)
                return;

            //SaveDialogClosed();
        }

        //private bool SaveDialogClosed()
        //{
        //    // Update default path etc
        //    string path = saveFileDialog1.FileName;
        //    string fileName = Path.GetFileName(path);
        //    string dir = Path.GetDirectoryName(path);

        //    // Possible states
        //    // fileName is a file - save dir
        //    // fileName is a dir - save dir

        //    if (File.Exists(path))
        //    {
        //        ExportSaveFolder = dir;
        //        return true;
        //    }
        //    else
        //    {

        //    }


        //    FileAttributes attr = File.GetAttributes(@"c:\Temp");
        //    if (Path.GetFileName)

        //    return true;
        //}

    }
}
