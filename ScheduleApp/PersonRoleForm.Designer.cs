namespace GuardScheduler
{
    partial class PersonRoleForm
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
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.comboBoxPrimaryRole = new System.Windows.Forms.ComboBox();
            this.comboBoxSecondaryRole = new System.Windows.Forms.ComboBox();
            this.checkBoxAvailable = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.labelFirstName = new System.Windows.Forms.Label();
            this.labelLastName = new System.Windows.Forms.Label();
            this.labelPrimaryRole = new System.Windows.Forms.Label();
            this.labelSecondaryRole = new System.Windows.Forms.Label();
            this.labelAvailable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtFirstName
            this.txtFirstName.Location = new System.Drawing.Point(150, 30);
            this.txtFirstName.Size = new System.Drawing.Size(200, 22);
            // 
            // txtLastName
            this.txtLastName.Location = new System.Drawing.Point(150, 70);
            this.txtLastName.Size = new System.Drawing.Size(200, 22);
            // 
            // comboBoxPrimaryRole
            this.comboBoxPrimaryRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPrimaryRole.Location = new System.Drawing.Point(150, 110);
            this.comboBoxPrimaryRole.Size = new System.Drawing.Size(200, 24);
            // 
            // comboBoxSecondaryRole
            this.comboBoxSecondaryRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSecondaryRole.Location = new System.Drawing.Point(150, 150);
            this.comboBoxSecondaryRole.Size = new System.Drawing.Size(200, 24);
            // 
            // checkBoxAvailable
            this.checkBoxAvailable.Location = new System.Drawing.Point(150, 190);
            this.checkBoxAvailable.Size = new System.Drawing.Size(20, 20);
            // 
            // btnSave
            this.btnSave.Location = new System.Drawing.Point(150, 230);
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Labels
            this.labelFirstName.Location = new System.Drawing.Point(30, 30);
            this.labelFirstName.Text = "First Name:";
            this.labelLastName.Location = new System.Drawing.Point(30, 70);
            this.labelLastName.Text = "Last Name:";
            this.labelPrimaryRole.Location = new System.Drawing.Point(30, 110);
            this.labelPrimaryRole.Text = "Primary Role:";
            this.labelSecondaryRole.Location = new System.Drawing.Point(30, 150);
            this.labelSecondaryRole.Text = "Secondary Role:";
            this.labelAvailable.Location = new System.Drawing.Point(30, 190);
            this.labelAvailable.Text = "Available:";
            // 
            // Form
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.comboBoxPrimaryRole);
            this.Controls.Add(this.comboBoxSecondaryRole);
            this.Controls.Add(this.checkBoxAvailable);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelFirstName);
            this.Controls.Add(this.labelLastName);
            this.Controls.Add(this.labelPrimaryRole);
            this.Controls.Add(this.labelSecondaryRole);
            this.Controls.Add(this.labelAvailable);
            this.Text = "Edit Person Roles & Availability";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.ComboBox comboBoxPrimaryRole;
        private System.Windows.Forms.ComboBox comboBoxSecondaryRole;
        private System.Windows.Forms.CheckBox checkBoxAvailable;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label labelFirstName;
        private System.Windows.Forms.Label labelLastName;
        private System.Windows.Forms.Label labelPrimaryRole;
        private System.Windows.Forms.Label labelSecondaryRole;
        private System.Windows.Forms.Label labelAvailable;
    }
}
