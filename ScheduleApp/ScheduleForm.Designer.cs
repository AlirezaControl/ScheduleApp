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
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.btnGenerateSchedule = new System.Windows.Forms.Button();
            this.listBoxSchedule = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // labelFromDate
            // 
            this.labelFromDate.AutoSize = true;
            this.labelFromDate.Location = new System.Drawing.Point(12, 15);
            this.labelFromDate.Name = "labelFromDate";
            this.labelFromDate.Size = new System.Drawing.Size(71, 17);
            this.labelFromDate.TabIndex = 0;
            this.labelFromDate.Text = "From Date:";
            // 
            // labelToDate
            // 
            this.labelToDate.AutoSize = true;
            this.labelToDate.Location = new System.Drawing.Point(12, 50);
            this.labelToDate.Name = "labelToDate";
            this.labelToDate.Size = new System.Drawing.Size(63, 17);
            this.labelToDate.TabIndex = 1;
            this.labelToDate.Text = "To Date:";
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Location = new System.Drawing.Point(90, 12);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(200, 22);
            this.dateTimePickerFrom.TabIndex = 2;
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Location = new System.Drawing.Point(90, 47);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(200, 22);
            this.dateTimePickerTo.TabIndex = 3;
            // 
            // btnGenerateSchedule
            // 
            this.btnGenerateSchedule.Location = new System.Drawing.Point(90, 80);
            this.btnGenerateSchedule.Name = "btnGenerateSchedule";
            this.btnGenerateSchedule.Size = new System.Drawing.Size(200, 30);
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
            this.listBoxSchedule.ItemHeight = 16;
            this.listBoxSchedule.Location = new System.Drawing.Point(15, 120);
            this.listBoxSchedule.Name = "listBoxSchedule";
            this.listBoxSchedule.Size = new System.Drawing.Size(275, 164);
            this.listBoxSchedule.TabIndex = 5;
            // 
            // ScheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 301);
            this.Controls.Add(this.listBoxSchedule);
            this.Controls.Add(this.btnGenerateSchedule);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Controls.Add(this.labelToDate);
            this.Controls.Add(this.labelFromDate);
            this.Name = "ScheduleForm";
            this.Text = "Schedule Generator";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label labelFromDate;
        private System.Windows.Forms.Label labelToDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.Button btnGenerateSchedule;
        private System.Windows.Forms.ListBox listBoxSchedule;
    }
}