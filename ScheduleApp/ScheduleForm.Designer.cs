using System;
using System.Drawing;
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
            this.listBoxSchedule = new System.Windows.Forms.ListBox();
            this.dateTimePickerFrom = new PersianDateTimeControl.PersianDatePicker();
            this.dateTimePickerTo = new PersianDateTimeControl.PersianDatePicker();
            this.SuspendLayout();
            // 
            // labelFromDate
            // 
            this.labelFromDate.AutoSize = true;
            this.labelFromDate.Location = new System.Drawing.Point(14, 19);
            this.labelFromDate.Name = "labelFromDate";
            this.labelFromDate.Size = new System.Drawing.Size(89, 20);
            this.labelFromDate.TabIndex = 0;
            this.labelFromDate.Text = "From Date:";
            // 
            // labelToDate
            // 
            this.labelToDate.AutoSize = true;
            this.labelToDate.Location = new System.Drawing.Point(14, 62);
            this.labelToDate.Name = "labelToDate";
            this.labelToDate.Size = new System.Drawing.Size(70, 20);
            this.labelToDate.TabIndex = 1;
            this.labelToDate.Text = "To Date:";
            // 
            // btnGenerateSchedule
            // 
            this.btnGenerateSchedule.Location = new System.Drawing.Point(101, 100);
            this.btnGenerateSchedule.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGenerateSchedule.Name = "btnGenerateSchedule";
            this.btnGenerateSchedule.Size = new System.Drawing.Size(225, 38);
            this.btnGenerateSchedule.TabIndex = 4;
            this.btnGenerateSchedule.Text = "Generate Schedule";
            this.btnGenerateSchedule.UseVisualStyleBackColor = true;
            this.btnGenerateSchedule.Click += new System.EventHandler(this.btnGenerateSchedule_Click);
            // 
            // listBoxSchedule
            // 
            this.listBoxSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxSchedule.FormattingEnabled = true;
            this.listBoxSchedule.ItemHeight = 20;
            this.listBoxSchedule.Location = new System.Drawing.Point(17, 150);
            this.listBoxSchedule.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listBoxSchedule.Name = "listBoxSchedule";
            this.listBoxSchedule.Size = new System.Drawing.Size(483, 204);
            this.listBoxSchedule.TabIndex = 5;
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
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
            this.dateTimePickerTo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.dateTimePickerTo.Location = new System.Drawing.Point(151, 62);
            this.dateTimePickerTo.MaximumSize = new System.Drawing.Size(500, 500);
            this.dateTimePickerTo.MinimumSize = new System.Drawing.Size(93, 19);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(189, 31);
            this.dateTimePickerTo.TabIndex = 6;
            this.dateTimePickerTo.Value = null;
            // 
            // ScheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 376);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Controls.Add(this.listBoxSchedule);
            this.Controls.Add(this.btnGenerateSchedule);
            this.Controls.Add(this.labelToDate);
            this.Controls.Add(this.labelFromDate);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ScheduleForm";
            this.Text = "Schedule Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label labelFromDate;
        private System.Windows.Forms.Label labelToDate;
        private System.Windows.Forms.Button btnGenerateSchedule;
        private System.Windows.Forms.ListBox listBoxSchedule;
        private PersianDatePicker dateTimePickerFrom;
        private PersianDatePicker dateTimePickerTo;
    }
}