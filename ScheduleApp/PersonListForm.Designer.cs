namespace GuardScheduler
{
    partial class PersonListForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewPersons;
        private System.Windows.Forms.Button btnRefresh;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dataGridViewPersons = new System.Windows.Forms.DataGridView();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPersons)).BeginInit();
            this.SuspendLayout();

            // 
            // dataGridViewPersons
            // 
            this.dataGridViewPersons.AllowUserToAddRows = false;
            this.dataGridViewPersons.AllowUserToDeleteRows = false;
            this.dataGridViewPersons.Anchor = ((System.Windows.Forms.AnchorStyles)
                ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPersons.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPersons.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewPersons.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewPersons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPersons.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewPersons.MultiSelect = false;
            this.dataGridViewPersons.Name = "dataGridViewPersons";
            this.dataGridViewPersons.ReadOnly = true;
            this.dataGridViewPersons.RowHeadersVisible = false;
            this.dataGridViewPersons.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPersons.Size = new System.Drawing.Size(860, 420);
            this.dataGridViewPersons.TabIndex = 0;
            this.dataGridViewPersons.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPersons_CellDoubleClick);

            // Define Columns (Persian headers)
            this.dataGridViewPersons.Columns.Add("FullName", "نام و نام خانوادگی");
            this.dataGridViewPersons.Columns.Add("PrimaryRole", "نقش اصلی");
            this.dataGridViewPersons.Columns.Add("SecondaryRole", "نقش دوم");
            this.dataGridViewPersons.Columns.Add("Married", "وضعیت تأهل");
            this.dataGridViewPersons.Columns.Add("Available", "وضعیت دسترسی");
            this.dataGridViewPersons.Columns.Add("RotationOrder", "ترتیب چرخش");

            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(760, 440);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(112, 35);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "تازه‌سازی";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // 
            // PersonListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.ClientSize = new System.Drawing.Size(884, 491);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dataGridViewPersons);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "PersonListForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "فهرست افراد";

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPersons)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
