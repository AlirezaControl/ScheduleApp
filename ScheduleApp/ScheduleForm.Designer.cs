using System;
using System.Windows.Forms;
using PersianDateTimeControl;

namespace GuardScheduler
{
    partial class ScheduleForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
        private Button btnExportWord;
        private void InitializeComponent()
        {
            this.labelFromDate = new System.Windows.Forms.Label();
            this.labelToDate = new System.Windows.Forms.Label();
            this.btnGenerateSchedule = new System.Windows.Forms.Button();
            this.btnOpenPersonList = new System.Windows.Forms.Button();
            this.dateTimePickerFrom = new PersianDateTimeControl.PersianDatePicker();
            this.dateTimePickerTo = new PersianDateTimeControl.PersianDatePicker();
            this.dgv24HourPosts = new System.Windows.Forms.DataGridView();
            this.dgvNegahban = new System.Windows.Forms.DataGridView();
            this.dgvPasbakhsh = new System.Windows.Forms.DataGridView();
            this.dgvDezhban = new System.Windows.Forms.DataGridView();
            this.grp24Hour = new System.Windows.Forms.GroupBox();
            this.grpNegahban = new System.Windows.Forms.GroupBox();
            this.grpPasbakhsh = new System.Windows.Forms.GroupBox();
            this.grpDezhban = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv24HourPosts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNegahban)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPasbakhsh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDezhban)).BeginInit();
            this.grp24Hour.SuspendLayout();
            this.grpNegahban.SuspendLayout();
            this.grpPasbakhsh.SuspendLayout();
            this.grpDezhban.SuspendLayout();
            this.SuspendLayout();

            this.btnExportWord = new System.Windows.Forms.Button();
            this.btnExportWord.Location = new System.Drawing.Point(520, 100);
            this.btnExportWord.Size = new System.Drawing.Size(150, 38);
            this.btnExportWord.Text = "خروجی Word";
            this.btnExportWord.Click += new System.EventHandler(this.btnExportWord_Click);
            this.Controls.Add(this.btnExportWord);

            // 
            // labelFromDate
            // 
            this.labelFromDate.AutoSize = true;
            this.labelFromDate.Location = new System.Drawing.Point(14, 19);
            this.labelFromDate.Name = "labelFromDate";
            this.labelFromDate.Size = new System.Drawing.Size(58, 20);
            this.labelFromDate.TabIndex = 9;
            this.labelFromDate.Text = "از تاریخ:";
            // 
            // labelToDate
            // 
            this.labelToDate.AutoSize = true;
            this.labelToDate.Location = new System.Drawing.Point(14, 62);
            this.labelToDate.Name = "labelToDate";
            this.labelToDate.Size = new System.Drawing.Size(54, 20);
            this.labelToDate.TabIndex = 8;
            this.labelToDate.Text = "تا تاریخ:";
            // 
            // btnGenerateSchedule
            // 
            this.btnGenerateSchedule.Location = new System.Drawing.Point(101, 100);
            this.btnGenerateSchedule.Name = "btnGenerateSchedule";
            this.btnGenerateSchedule.Size = new System.Drawing.Size(225, 38);
            this.btnGenerateSchedule.TabIndex = 7;
            this.btnGenerateSchedule.Text = "تولید برنامه";
            this.btnGenerateSchedule.Click += new System.EventHandler(this.btnGenerateSchedule_Click);
            // 
            // btnOpenPersonList
            // 
            this.btnOpenPersonList.Location = new System.Drawing.Point(350, 100);
            this.btnOpenPersonList.Name = "btnOpenPersonList";
            this.btnOpenPersonList.Size = new System.Drawing.Size(150, 38);
            this.btnOpenPersonList.TabIndex = 0;
            this.btnOpenPersonList.Text = "لیست افراد";
            this.btnOpenPersonList.Click += new System.EventHandler(this.btnOpenPersonList_Click);
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.dateTimePickerFrom.Location = new System.Drawing.Point(151, 19);
            this.dateTimePickerFrom.MaximumSize = new System.Drawing.Size(93, 19);
            this.dateTimePickerFrom.MinimumSize = new System.Drawing.Size(93, 19);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(93, 19);
            this.dateTimePickerFrom.TabIndex = 6;
            this.dateTimePickerFrom.Value = null;
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.dateTimePickerTo.Location = new System.Drawing.Point(151, 62);
            this.dateTimePickerTo.MaximumSize = new System.Drawing.Size(93, 19);
            this.dateTimePickerTo.MinimumSize = new System.Drawing.Size(93, 19);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(93, 19);
            this.dateTimePickerTo.TabIndex = 5;
            this.dateTimePickerTo.Value = null;
            // 
            // dgv24HourPosts
            // 
            this.dgv24HourPosts.Location = new System.Drawing.Point(6, 25);
            this.dgv24HourPosts.Name = "dgv24HourPosts";
            this.dgv24HourPosts.Size = new System.Drawing.Size(669, 150);
            this.dgv24HourPosts.TabIndex = 0;
            // 
            // dgvNegahban
            // 
            this.dgvNegahban.Location = new System.Drawing.Point(6, 25);
            this.dgvNegahban.Name = "dgvNegahban";
            this.dgvNegahban.Size = new System.Drawing.Size(669, 150);
            this.dgvNegahban.TabIndex = 0;
            // 
            // dgvPasbakhsh
            // 
            this.dgvPasbakhsh.Location = new System.Drawing.Point(6, 25);
            this.dgvPasbakhsh.Name = "dgvPasbakhsh";
            this.dgvPasbakhsh.Size = new System.Drawing.Size(688, 150);
            this.dgvPasbakhsh.TabIndex = 0;
            // 
            // dgvDezhban
            // 
            this.dgvDezhban.Location = new System.Drawing.Point(6, 25);
            this.dgvDezhban.Name = "dgvDezhban";
            this.dgvDezhban.Size = new System.Drawing.Size(669, 150);
            this.dgvDezhban.TabIndex = 0;
            // 
            // grp24Hour
            // 
            this.grp24Hour.Controls.Add(this.dgv24HourPosts);
            this.grp24Hour.Location = new System.Drawing.Point(17, 150);
            this.grp24Hour.Name = "grp24Hour";
            this.grp24Hour.Size = new System.Drawing.Size(700, 200);
            this.grp24Hour.TabIndex = 4;
            this.grp24Hour.TabStop = false;
            this.grp24Hour.Text = "پست‌های ۲۴ ساعته";
            // 
            // grpNegahban
            // 
            this.grpNegahban.Controls.Add(this.dgvNegahban);
            this.grpNegahban.Location = new System.Drawing.Point(17, 360);
            this.grpNegahban.Name = "grpNegahban";
            this.grpNegahban.Size = new System.Drawing.Size(700, 200);
            this.grpNegahban.TabIndex = 3;
            this.grpNegahban.TabStop = false;
            this.grpNegahban.Text = "نگهبان";
            // 
            // grpPasbakhsh
            // 
            this.grpPasbakhsh.Controls.Add(this.dgvPasbakhsh);
            this.grpPasbakhsh.Location = new System.Drawing.Point(17, 570);
            this.grpPasbakhsh.Name = "grpPasbakhsh";
            this.grpPasbakhsh.Size = new System.Drawing.Size(700, 200);
            this.grpPasbakhsh.TabIndex = 2;
            this.grpPasbakhsh.TabStop = false;
            this.grpPasbakhsh.Text = "پاس‌بخش";
            // 
            // grpDezhban
            // 
            this.grpDezhban.Controls.Add(this.dgvDezhban);
            this.grpDezhban.Location = new System.Drawing.Point(17, 780);
            this.grpDezhban.Name = "grpDezhban";
            this.grpDezhban.Size = new System.Drawing.Size(700, 200);
            this.grpDezhban.TabIndex = 1;
            this.grpDezhban.TabStop = false;
            this.grpDezhban.Text = "دژبان";
            // 
            // ScheduleForm
            // 
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(774, 1000);
            this.Controls.Add(this.btnOpenPersonList);
            this.Controls.Add(this.grpDezhban);
            this.Controls.Add(this.grpPasbakhsh);
            this.Controls.Add(this.grpNegahban);
            this.Controls.Add(this.grp24Hour);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Controls.Add(this.btnGenerateSchedule);
            this.Controls.Add(this.labelToDate);
            this.Controls.Add(this.labelFromDate);
            this.Name = "ScheduleForm";
            this.Text = "برنامه‌ریزی نگهبانی";
            ((System.ComponentModel.ISupportInitialize)(this.dgv24HourPosts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNegahban)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPasbakhsh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDezhban)).EndInit();
            this.grp24Hour.ResumeLayout(false);
            this.grpNegahban.ResumeLayout(false);
            this.grpPasbakhsh.ResumeLayout(false);
            this.grpDezhban.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label labelFromDate;
        private Label labelToDate;
        private Button btnGenerateSchedule;
        private Button btnOpenPersonList;
        private PersianDatePicker dateTimePickerFrom;
        private PersianDatePicker dateTimePickerTo;

        private GroupBox grp24Hour;
        private GroupBox grpNegahban;
        private GroupBox grpPasbakhsh;
        private GroupBox grpDezhban;

        private DataGridView dgv24HourPosts;
        private DataGridView dgvNegahban;
        private DataGridView dgvPasbakhsh;
        private DataGridView dgvDezhban;
    }
}
