using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGrid;

namespace Dek.Cls.SyncFusion
{
    public class SfDataGridHelper
    {
        /*
        https://www.syncfusion.com/kb/11302/how-to-get-a-cell-value-in-detailsviewdatagrid-in-datagridsfdatagrid
        Get value from cell. 
        Gets it from record via reflection, which is a ReportModel in the IEnumerable<ReportModel>
        
        Should work with grouping.
            Grouping seems to be a coillection of sfDataGrids, which is nice.

            Example calling code: 

            private void BtnGetCellValue_Click(object sender, EventArgs e)
            {      
                SfDataGrid sfDataGrid = this.sfDataGrid1.GetDetailsViewGrid(2);
                SfDataGrid secondlevel = sfDataGrid.GetDetailsViewGrid(2);
                txtGetCellValue.Text = GetCellValue(secondlevel, 1, 1);
            }
        
        */
        public static string GetCellValue(SfDataGrid sfDataGrid, int rowIdx, int colIdx)
        {
            if (colIdx < 0)
                return string.Empty;

            var mappingName = sfDataGrid.Columns[colIdx].MappingName;

            return GetCellValue(sfDataGrid, rowIdx, mappingName);
        }

        public static string GetCellValue(SfDataGrid sfDataGrid, int rowIdx, string colName)
        {
            string cellValue;

            //var recordIndex = sfDataGrid.TableControl.ResolveToRecordIndex(rowIdx);
            //((Dek.Bel.Core.Services.ReportModel)sfDataGrid.View.GetRecordAt(2).Data).OriginalCitation
            
            int recordIdx = rowIdx;
            if (recordIdx < 0)
                return string.Empty;

            if (sfDataGrid.View.TopLevelGroup != null)
            {
                var record = sfDataGrid.View.TopLevelGroup.DisplayElements[recordIdx];
                if (!record.IsRecords)
                    return string.Empty;

                var data = (record as RecordEntry).Data;
                cellValue = data.GetType().GetProperty(colName).GetValue(data, null)?.ToString() ?? string.Empty;
            }
            else
            {
                var record1 = sfDataGrid.View.Records.GetItemAt(recordIdx);
                cellValue = record1.GetType().GetProperty(colName).GetValue(record1, null)?.ToString() ?? string.Empty;
            }

            return cellValue;
        }
    }
}