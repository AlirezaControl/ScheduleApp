using System.Data;
using System.Linq;
using ClosedXML.Excel;

namespace GuardScheduler.Services
{
    public static class ExcelHelper
    {
        /// <summary>
        /// Reads the first worksheet of an Excel file and returns a DataTable
        /// </summary>
        public static DataTable ReadExcel(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheets.First();
            var dt = new DataTable();

            // Read header
            foreach (var cell in worksheet.Row(1).Cells())
                dt.Columns.Add(cell.Value.ToString());

            // Read data rows
            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                var dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                    dr[i] = row.Cell(i + 1).Value.ToString();
                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}