using System.Drawing;
using System.Windows.Forms;

namespace GuardScheduler.UI
{
    partial class DetailedScheduleForm
    {
        private void InitializeTables()
        {
            Color headerBackColor = Color.FromArgb(30, 144, 255);
            Color headerForeColor = Color.White;
            Font headerFont = new Font("Tahoma", 11, FontStyle.Bold);
            Font cellFont = new Font("Tahoma", 10);

            tablePasbakhsh = CreateTable(6, 2, 920, 70);
            string[] pasbakhshHeaders = { "زمان شیفت", "06-14", "14-18", "18-22", "22-02", "02-06" };
            for (int i = 0; i < 6; i++)
                AddCellToTable(tablePasbakhsh, pasbakhshHeaders[i], i, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tablePasbakhsh, "نام پاسبخش", 0, 1, Color.FromArgb(240, 248, 255), Color.Black, cellFont, false);
            for (int i = 1; i < 6; i++)
                tablePasbakhsh.Controls.Add(CreateSelectableLabel($"p{i}"), i, 1);

            tableDezhbanMorning = CreateTable(6, 2, 920, 70);
            string[] dezhbanMorningHeaders = { "زمان شیفت", "08-10", "10-12", "12-14", "14-16", "16-18" };
            for (int i = 0; i < 6; i++)
                AddCellToTable(tableDezhbanMorning, dezhbanMorningHeaders[i], i, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanMorning, "نام دژبان", 0, 1, Color.FromArgb(240, 248, 255), Color.Black, cellFont, false);
            for (int i = 1; i < 6; i++)
                tableDezhbanMorning.Controls.Add(CreateSelectableLabel($"d{i}"), i, 1);

            tableDezhbanEvening = CreateTable(6, 2, 920, 70);
            string[] dezhbanEveningHeaders = { "زمان شیفت", "20-22", "22-00", "00-02", "02-04", "04-06" };
            for (int i = 0; i < 6; i++)
                AddCellToTable(tableDezhbanEvening, dezhbanEveningHeaders[i], i, 0, headerBackColor, headerForeColor, headerFont, true);
            AddCellToTable(tableDezhbanEvening, "نام دژبان", 0, 1, Color.FromArgb(240, 248, 255), Color.Black, cellFont, false);
            for (int i = 1; i < 6; i++)
                tableDezhbanEvening.Controls.Add(CreateSelectableLabel($"d{i + 6}"), i, 1);

            tableDezhbanCombined = CreateTable(7, 7, 920, 230);
            string[] combinedHeaders = { "ساعت", "ضلع دژبانی (صبح)", "ضلع دژبانی (عصر)", "ضلع شرقی (صبح)", "ضلع شرقی (عصر)", "ضلع غربی (صبح)", "ضلع غربی (عصر)" };
            for (int i = 0; i < 7; i++)
                AddCellToTable(tableDezhbanCombined, combinedHeaders[i], i, 0, headerBackColor, headerForeColor, headerFont, true);

            string[] hours = { "06-08", "08-10", "10-12", "12-02", "02-04", "04-06" };
            for (int row = 1; row <= 6; row++)
            {
                AddCellToTable(tableDezhbanCombined, hours[row - 1], 0, row, Color.FromArgb(240, 248, 255), Color.Black, cellFont, false);

                tableDezhbanCombined.Controls.Add(CreateSelectableLabel($"nd{row}"), 1, row);
                tableDezhbanCombined.Controls.Add(CreateSelectableLabel($"nd{row + 6}"), 2, row);
                tableDezhbanCombined.Controls.Add(CreateSelectableLabel($"sh{row}"), 3, row);
                tableDezhbanCombined.Controls.Add(CreateSelectableLabel($"sh{row + 6}"), 4, row);
                tableDezhbanCombined.Controls.Add(CreateSelectableLabel($"gh{row}"), 5, row);
                tableDezhbanCombined.Controls.Add(CreateSelectableLabel($"gh{row + 6}"), 6, row);
            }
        }

        private TableLayoutPanel CreateTable(int cols, int rows, int width, int height)
        {
            var t = new TableLayoutPanel()
            {
                ColumnCount = cols,
                RowCount = rows,
                Width = width,
                Height = height,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                BackColor = Color.White
            };
            for (int i = 0; i < cols; i++)
                t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / cols));
            for (int i = 0; i < rows; i++)
                t.RowStyles.Add(new RowStyle(SizeType.Absolute, height / rows));
            return t;
        }

        private void InitializeBottomTable()
        {
            tableBottom = CreateTable(6, 6, 920, 180);
            Font headerFont = new Font("Tahoma", 10, FontStyle.Bold);
            Font cellFont = new Font("Tahoma", 10);
            Color headerBackColor = Color.FromArgb(30, 144, 255);
            Color headerForeColor = Color.White;

            AddCellToTable(tableBottom, "مسئول بازداشتگاه", 0, 0, headerBackColor, headerForeColor, headerFont, true);
            tableBottom.Controls.Add(CreateSelectableLabel("na"), 3, 0);

            AddCellToTable(tableBottom, "راننده آماده", 0, 1, headerBackColor, headerForeColor, headerFont, true);
            tableBottom.Controls.Add(CreateSelectableLabel("R"), 1, 1);

            AddCellToTable(tableBottom, "مسئول نظافت: حمام/سلف/سرویس", 0, 2, headerBackColor, headerForeColor, headerFont, true);
            tableBottom.Controls.Add(CreateSelectableLabel("mn"), 1, 2);

            AddCellToTable(tableBottom, "افسر قرارگاه", 2, 3, headerBackColor, headerForeColor, headerFont, true);
            tableBottom.Controls.Add(CreateSelectableLabel("agh"), 3, 3);

            AddCellToTable(tableBottom, "شیفت آشپزخانه", 2, 4, headerBackColor, headerForeColor, headerFont, true);
            tableBottom.Controls.Add(CreateSelectableLabel("a1/a2"), 3, 4);
        }
    }
}