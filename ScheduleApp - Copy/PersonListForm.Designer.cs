namespace GuardScheduler
{
    partial class PersonListForm
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
            this.listBoxPersons = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxPersons
            // 
            this.listBoxPersons.FormattingEnabled = true;
            this.listBoxPersons.ItemHeight = 16;
            this.listBoxPersons.Location = new System.Drawing.Point(12, 12);
            this.listBoxPersons.Name = "listBoxPersons";
            this.listBoxPersons.Size = new System.Drawing.Size(360, 244);
            this.listBoxPersons.TabIndex = 0;
            this.listBoxPersons.DoubleClick += new System.EventHandler(this.listBoxPersons_DoubleClick);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(12, 262);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 30);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // PersonListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 301);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.listBoxPersons);
            this.Name = "PersonListForm";
            this.Text = "Person List";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ListBox listBoxPersons;
        private System.Windows.Forms.Button btnRefresh;
    }
}