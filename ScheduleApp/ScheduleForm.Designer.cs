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
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelFromDate = new System.Windows.Forms.Label();
            this.labelToDate = new System.Windows.Forms.Label();
            this.btnGenerateSchedule = new System.Windows.Forms.Button();
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
            // 
            // labelFromDate
            // 
            this.labelFromDate.AutoSize = true;
            this.labelFromDate.Location = new System.Drawing.Point(14, 19);
            this.labelFromDate.Name = "labelFromDate";
            this.labelFromDate.Size = new System.Drawing.Size(58, 20);
            this.labelFromDate.TabIndex = 0;
            this.labelFromDate.Text = "از تاریخ:";
            // 
            // labelToDate
            // 
            this.labelToDate.AutoSize = true;
            this.labelToDate.Location = new System.Drawing.Point(14, 62);
            this.labelToDate.Name = "labelToDate";
            this.labelToDate.Size = new System.Drawing.Size(54, 20);
            this.labelToDate.TabIndex = 1;
            this.labelToDate.Text = "تا تاریخ:";
            // 
            // btnGenerateSchedule
            // 
            this.btnGenerateSchedule.Location = new System.Drawing.Point(101, 100);
            this.btnGenerateSchedule.Name = "btnGenerateSchedule";
            this.btnGenerateSchedule.Size = new System.Drawing.Size(225, 38);
            this.btnGenerateSchedule.TabIndex = 4;
            this.btnGenerateSchedule.Text = "تولید برنامه";
            this.btnGenerateSchedule.UseVisualStyleBackColor = true;
            this.btnGenerateSchedule.Click += new System.EventHandler(this.btnGenerateSchedule_Click);
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dateTimePickerFrom.Location = new System.Drawing.Point(151, 19);
            this.dateTimePickerFrom.MaximumSize = new System.Drawing.Size(500, 500);
            this.dateTimePickerFrom.MinimumSize = new System.Drawing.Size(93, 19);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(189, 31);
            this.dateTimePickerFrom.TabIndex = 6;
            this.dateTimePickerFrom.Value = null;
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dateTimePickerTo.Location = new System.Drawing.Point(151, 62);
            this.dateTimePickerTo.MaximumSize = new System.Drawing.Size(500, 500);
            this.dateTimePickerTo.MinimumSize = new System.Drawing.Size(93, 19);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(189, 31);
            this.dateTimePickerTo.TabIndex = 7;
            this.dateTimePickerTo.Value = null;
            // 
            // dgv24HourPosts
            // 
            this.dgv24HourPosts.AllowUserToAddRows = false;
            this.dgv24HourPosts.AllowUserToDeleteRows = false;
            this.dgv24HourPosts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv24HourPosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv24HourPosts.Location = new System.Drawing.Point(3, 22);
            this.dgv24HourPosts.Name = "dgv24HourPosts";
            this.dgv24HourPosts.ReadOnly = true;
            this.dgv24HourPosts.Size = new System.Drawing.Size(694, 175);
            this.dgv24HourPosts.TabIndex = 0;
            // 
            // dgvNegahban
            // 
            this.dgvNegahban.AllowUserToAddRows = false;
            this.dgvNegahban.AllowUserToDeleteRows = false;
            this.dgvNegahban.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNegahban.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNegahban.Location = new System.Drawing.Point(3, 22);
            this.dgvNegahban.Name = "dgvNegahban";
            this.dgvNegahban.ReadOnly = true;
            this.dgvNegahban.Size = new System.Drawing.Size(694, 175);
            this.dgvNegahban.TabIndex = 0;
            // 
            // dgvPasbakhsh
            // 
            this.dgvPasbakhsh.AllowUserToAddRows = false;
            this.dgvPasbakhsh.AllowUserToDeleteRows = false;
            this.dgvPasbakhsh.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPasbakhsh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPasbakhsh.Location = new System.Drawing.Point(3, 22);
            this.dgvPasbakhsh.Name = "dgvPasbakhsh";
            this.dgvPasbakhsh.ReadOnly = true;
            this.dgvPasbakhsh.Size = new System.Drawing.Size(694, 175);
            this.dgvPasbakhsh.TabIndex = 0;
            // 
            // dgvDezhban
            // 
            this.dgvDezhban.AllowUserToAddRows = false;
            this.dgvDezhban.AllowUserToDeleteRows = false;
            this.dgvDezhban.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDezhban.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDezhban.Location = new System.Drawing.Point(3, 22);
            this.dgvDezhban.Name = "dgvDezhban";
            this.dgvDezhban.ReadOnly = true;
            this.dgvDezhban.Size = new System.Drawing.Size(694, 175);
            this.dgvDezhban.TabIndex = 0;
            // 
            // grp24Hour
            // 
            this.grp24Hour.Controls.Add(this.dgv24HourPosts);
            this.grp24Hour.Location = new System.Drawing.Point(17, 150);
            this.grp24Hour.Name = "grp24Hour";
            this.grp24Hour.Size = new System.Drawing.Size(700, 200);
            this.grp24Hour.TabIndex = 8;
            this.grp24Hour.Text = "پست‌های ۲۴ ساعته";
            // 
            // grpNegahban
            // 
            this.grpNegahban.Controls.Add(this.dgvNegahban);
            this.grpNegahban.Location = new System.Drawing.Point(17, 360);
            this.grpNegahban.Name = "grpNegahban";
            this.grpNegahban.Size = new System.Drawing.Size(700, 200);
            this.grpNegahban.TabIndex = 9;
            this.grpNegahban.Text = "نگهبان";
            // 
            // grpPasbakhsh
            // 
            this.grpPasbakhsh.Controls.Add(this.dgvPasbakhsh);
            this.grpPasbakhsh.Location = new System.Drawing.Point(17, 570);
            this.grpPasbakhsh.Name = "grpPasbakhsh";
            this.grpPasbakhsh.Size = new System.Drawing.Size(700, 200);
            this.grpPasbakhsh.TabIndex = 10;
            this.grpPasbakhsh.Text = "پاس‌بخش";
            // 
            // grpDezhban
            // 
            this.grpDezhban.Controls.Add(this.dgvDezhban);
            this.grpDezhban.Location = new System.Drawing.Point(17, 780);
            this.grpDezhban.Name = "grpDezhban";
            this.grpDezhban.Size = new System.Drawing.Size(700, 200);
            this.grpDezhban.TabIndex = 11;
            this.grpDezhban.Text = "دژبان";
            // 
            // ScheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1305, 1000);
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

        private System.Windows.Forms.Label labelFromDate;
        private System.Windows.Forms.Label labelToDate;
        private System.Windows.Forms.Button btnGenerateSchedule;
        private PersianDatePicker dateTimePickerFrom;
        private PersianDatePicker dateTimePickerTo;

        private System.Windows.Forms.GroupBox grp24Hour;
        private System.Windows.Forms.GroupBox grpNegahban;
        private System.Windows.Forms.GroupBox grpPasbakhsh;
        private System.Windows.Forms.GroupBox grpDezhban;

        private System.Windows.Forms.DataGridView dgv24HourPosts;
        private System.Windows.Forms.DataGridView dgvNegahban;
        private System.Windows.Forms.DataGridView dgvPasbakhsh;
        private System.Windows.Forms.DataGridView dgvDezhban;
    }
}
