using MD.PersianDateTime;
using PersianDateTimeControl;

namespace GuardScheduler
{
    partial class ScheduleForm
    {
        private System.ComponentModel.IContainer components = null;
        private PersianDateTimeControl.PersianDatePicker dateTimePickerFrom;
        private PersianDateTimeControl.PersianDatePicker dateTimePickerTo;
        private System.Windows.Forms.Button btnGenerateSchedule;
        private System.Windows.Forms.Button btnExportWord;
        private System.Windows.Forms.Button btnOpenPersonList;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DataGridView dgv24HourPosts;
        private System.Windows.Forms.DataGridView dgvNegahban;
        private System.Windows.Forms.DataGridView dgvPasbakhsh;
        private System.Windows.Forms.DataGridView dgvDezhban;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tab24Hour;
        private System.Windows.Forms.TabPage tabNegahban;
        private System.Windows.Forms.TabPage tabPasbakhsh;
        private System.Windows.Forms.TabPage tabDezhban;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dateTimePickerFrom = new PersianDateTimeControl.PersianDatePicker();
            this.dateTimePickerTo = new PersianDateTimeControl.PersianDatePicker();
            this.btnGenerateSchedule = new System.Windows.Forms.Button();
            this.btnExportWord = new System.Windows.Forms.Button();
            this.btnOpenPersonList = new System.Windows.Forms.Button();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tab24Hour = new System.Windows.Forms.TabPage();
            this.dgv24HourPosts = new System.Windows.Forms.DataGridView();
            this.tabNegahban = new System.Windows.Forms.TabPage();
            this.dgvNegahban = new System.Windows.Forms.DataGridView();
            this.tabPasbakhsh = new System.Windows.Forms.TabPage();
            this.dgvPasbakhsh = new System.Windows.Forms.DataGridView();
            this.tabDezhban = new System.Windows.Forms.TabPage();
            this.dgvDezhban = new System.Windows.Forms.DataGridView();

            this.tabControl.SuspendLayout();
            this.tab24Hour.SuspendLayout();
            this.tabNegahban.SuspendLayout();
            this.tabPasbakhsh.SuspendLayout();
            this.tabDezhban.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv24HourPosts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNegahban)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPasbakhsh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDezhban)).BeginInit();
            this.SuspendLayout();

            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(680, 20);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(55, 13);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "از تاریخ:";

            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(500, 20);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(55, 13);
            this.lblTo.TabIndex = 1;
            this.lblTo.Text = "تا تاریخ:";

            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Location = new System.Drawing.Point(570, 15);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(100, 21);
            this.dateTimePickerFrom.TabIndex = 2;

            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Location = new System.Drawing.Point(390, 15);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(100, 21);
            this.dateTimePickerTo.TabIndex = 3;

            // 
            // btnGenerateSchedule
            // 
            this.btnGenerateSchedule.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGenerateSchedule.ForeColor = System.Drawing.Color.White;
            this.btnGenerateSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateSchedule.Location = new System.Drawing.Point(260, 10);
            this.btnGenerateSchedule.Name = "btnGenerateSchedule";
            this.btnGenerateSchedule.Size = new System.Drawing.Size(110, 30);
            this.btnGenerateSchedule.TabIndex = 4;
            this.btnGenerateSchedule.Text = "تولید برنامه";
            this.btnGenerateSchedule.UseVisualStyleBackColor = false;
            this.btnGenerateSchedule.Click += new System.EventHandler(this.btnGenerateSchedule_Click);

            // 
            // btnExportWord
            // 
            this.btnExportWord.BackColor = System.Drawing.Color.SeaGreen;
            this.btnExportWord.ForeColor = System.Drawing.Color.White;
            this.btnExportWord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportWord.Location = new System.Drawing.Point(140, 10);
            this.btnExportWord.Name = "btnExportWord";
            this.btnExportWord.Size = new System.Drawing.Size(110, 30);
            this.btnExportWord.TabIndex = 5;
            this.btnExportWord.Text = "خروجی Word";
            this.btnExportWord.UseVisualStyleBackColor = false;
            this.btnExportWord.Click += new System.EventHandler(this.btnExportWord_Click);

            // 
            // btnOpenPersonList
            // 
            this.btnOpenPersonList.BackColor = System.Drawing.Color.Gray;
            this.btnOpenPersonList.ForeColor = System.Drawing.Color.White;
            this.btnOpenPersonList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenPersonList.Location = new System.Drawing.Point(20, 10);
            this.btnOpenPersonList.Name = "btnOpenPersonList";
            this.btnOpenPersonList.Size = new System.Drawing.Size(110, 30);
            this.btnOpenPersonList.TabIndex = 6;
            this.btnOpenPersonList.Text = "مدیریت نفرات";
            this.btnOpenPersonList.UseVisualStyleBackColor = false;
            this.btnOpenPersonList.Click += new System.EventHandler(this.btnOpenPersonList_Click);

            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tab24Hour);
            this.tabControl.Controls.Add(this.tabNegahban);
            this.tabControl.Controls.Add(this.tabPasbakhsh);
            this.tabControl.Controls.Add(this.tabDezhban);
            this.tabControl.Location = new System.Drawing.Point(10, 50);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(760, 480);
            this.tabControl.TabIndex = 7;

            // 
            // tab24Hour
            // 
            this.tab24Hour.Controls.Add(this.dgv24HourPosts);
            this.tab24Hour.Location = new System.Drawing.Point(4, 22);
            this.tab24Hour.Name = "tab24Hour";
            this.tab24Hour.Padding = new System.Windows.Forms.Padding(3);
            this.tab24Hour.Size = new System.Drawing.Size(752, 454);
            this.tab24Hour.TabIndex = 0;
            this.tab24Hour.Text = "پست‌های ۲۴ ساعته";
            this.tab24Hour.UseVisualStyleBackColor = true;

            // 
            // dgv24HourPosts
            // 
            this.dgv24HourPosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv24HourPosts.Location = new System.Drawing.Point(3, 3);
            this.dgv24HourPosts.Name = "dgv24HourPosts";
            this.dgv24HourPosts.Size = new System.Drawing.Size(746, 448);
            this.dgv24HourPosts.TabIndex = 0;

            // 
            // tabNegahban
            // 
            this.tabNegahban.Controls.Add(this.dgvNegahban);
            this.tabNegahban.Location = new System.Drawing.Point(4, 22);
            this.tabNegahban.Name = "tabNegahban";
            this.tabNegahban.Padding = new System.Windows.Forms.Padding(3);
            this.tabNegahban.Size = new System.Drawing.Size(752, 454);
            this.tabNegahban.TabIndex = 1;
            this.tabNegahban.Text = "نگهبان";
            this.tabNegahban.UseVisualStyleBackColor = true;

            // 
            // dgvNegahban
            // 
            this.dgvNegahban.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNegahban.Location = new System.Drawing.Point(3, 3);
            this.dgvNegahban.Name = "dgvNegahban";
            this.dgvNegahban.Size = new System.Drawing.Size(746, 448);
            this.dgvNegahban.TabIndex = 0;

            // 
            // tabPasbakhsh
            // 
            this.tabPasbakhsh.Controls.Add(this.dgvPasbakhsh);
            this.tabPasbakhsh.Location = new System.Drawing.Point(4, 22);
            this.tabPasbakhsh.Name = "tabPasbakhsh";
            this.tabPasbakhsh.Padding = new System.Windows.Forms.Padding(3);
            this.tabPasbakhsh.Size = new System.Drawing.Size(752, 454);
            this.tabPasbakhsh.TabIndex = 2;
            this.tabPasbakhsh.Text = "پاسبخش";
            this.tabPasbakhsh.UseVisualStyleBackColor = true;

            // 
            // dgvPasbakhsh
            // 
            this.dgvPasbakhsh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPasbakhsh.Location = new System.Drawing.Point(3, 3);
            this.dgvPasbakhsh.Name = "dgvPasbakhsh";
            this.dgvPasbakhsh.Size = new System.Drawing.Size(746, 448);
            this.dgvPasbakhsh.TabIndex = 0;

            // 
            // tabDezhban
            // 
            this.tabDezhban.Controls.Add(this.dgvDezhban);
            this.tabDezhban.Location = new System.Drawing.Point(4, 22);
            this.tabDezhban.Name = "tabDezhban";
            this.tabDezhban.Padding = new System.Windows.Forms.Padding(3);
            this.tabDezhban.Size = new System.Drawing.Size(752, 454);
            this.tabDezhban.TabIndex = 3;
            this.tabDezhban.Text = "دژبان";
            this.tabDezhban.UseVisualStyleBackColor = true;

            // 
            // dgvDezhban
            // 
            this.dgvDezhban.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDezhban.Location = new System.Drawing.Point(3, 3);
            this.dgvDezhban.Name = "dgvDezhban";
            this.dgvDezhban.Size = new System.Drawing.Size(746, 448);
            this.dgvDezhban.TabIndex = 0;

            // 
            // ScheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(784, 541);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnOpenPersonList);
            this.Controls.Add(this.btnExportWord);
            this.Controls.Add(this.btnGenerateSchedule);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Name = "ScheduleForm";
            this.Text = "لوحه نگهبانی مرکز فاوا";

            this.tabControl.ResumeLayout(false);
            this.tab24Hour.ResumeLayout(false);
            this.tabNegahban.ResumeLayout(false);
            this.tabPasbakhsh.ResumeLayout(false);
            this.tabDezhban.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv24HourPosts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNegahban)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPasbakhsh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDezhban)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
