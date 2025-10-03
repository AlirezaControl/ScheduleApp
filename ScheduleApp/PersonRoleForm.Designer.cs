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
            this.btnSave = new System.Windows.Forms.Button();
            this.labelFirstName = new System.Windows.Forms.Label();
            this.labelLastName = new System.Windows.Forms.Label();
            this.labelPrimaryRole = new System.Windows.Forms.Label();
            this.labelSecondaryRole = new System.Windows.Forms.Label();
            this.checkBoxAvailable = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(150, 30);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(200, 22);
            this.txtFirstName.TabIndex = 0;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(150, 70);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(200, 22);
            this.txtLastName.TabIndex = 1;
            // 
            // comboBoxPrimaryRole
            // 
            this.comboBoxPrimaryRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPrimaryRole.Location = new System.Drawing.Point(150, 110);
            this.comboBoxPrimaryRole.Name = "comboBoxPrimaryRole";
            this.comboBoxPrimaryRole.Size = new System.Drawing.Size(200, 24);
            this.comboBoxPrimaryRole.TabIndex = 2;
            // 
            // comboBoxSecondaryRole
            // 
            this.comboBoxSecondaryRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSecondaryRole.Location = new System.Drawing.Point(150, 150);
            this.comboBoxSecondaryRole.Name = "comboBoxSecondaryRole";
            this.comboBoxSecondaryRole.Size = new System.Drawing.Size(200, 24);
            this.comboBoxSecondaryRole.TabIndex = 3;
            // 
            // checkBoxAvailable
            // 
            this.checkBoxAvailable.AutoSize = true;
            this.checkBoxAvailable.Location = new System.Drawing.Point(150, 190);
            this.checkBoxAvailable.Name = "checkBoxAvailable";
            this.checkBoxAvailable.Size = new System.Drawing.Size(85, 21);
            this.checkBoxAvailable.TabIndex = 4;
            this.checkBoxAvailable.Text = "Available";
            this.checkBoxAvailable.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(150, 230);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Labels
            // 
            this.labelFirstName.AutoSize = true;
            this.labelFirstName.Location = new System.Drawing.Point(30, 30);
            this.labelFirstName.Name = "labelFirstName";
            this.labelFirstName.Size = new System.Drawing.Size(78, 17);
            this.labelFirstName.TabIndex = 6;
            this.labelFirstName.Text = "First Name:";
            //
            this.labelLastName.AutoSize = true;
            this.labelLastName.Location = new System.Drawing.Point(30, 70);
            this.labelLastName.Name = "labelLastName";
            this.labelLastName.Size = new System.Drawing.Size(78, 17);
            this.labelLastName.TabIndex = 7;
            this.labelLastName.Text = "Last Name:";
            //
            this.labelPrimaryRole.AutoSize = true;
            this.labelPrimaryRole.Location = new System.Drawing.Point(30, 110);
            this.labelPrimaryRole.Name = "labelPrimaryRole";
            this.labelPrimaryRole.Size = new System.Drawing.Size(91, 17);
            this.labelPrimaryRole.TabIndex = 8;
            this.labelPrimaryRole.Text = "Primary Role:";
            //
            this.labelSecondaryRole.AutoSize = true;
            this.labelSecondaryRole.Location = new System.Drawing.Point(30, 150);
            this.labelSecondaryRole.Name = "labelSecondaryRole";
            this.labelSecondaryRole.Size = new System.Drawing.Size(108, 17);
            this.labelSecondaryRole.TabIndex = 9;
            this.labelSecondaryRole.Text = "Secondary Role:";
            // 
            // PersonRoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.labelSecondaryRole);
            this.Controls.Add(this.labelPrimaryRole);
            this.Controls.Add(this.labelLastName);
            this.Controls.Add(this.labelFirstName);
            this.Controls.Add(this.checkBoxAvailable);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.comboBoxSecondaryRole);
            this.Controls.Add(this.comboBoxPrimaryRole);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.txtFirstName);
            this.Name = "PersonRoleForm";
            this.Text = "Edit Person Roles & Availability";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.ComboBox comboBoxPrimaryRole;
        private System.Windows.Forms.ComboBox comboBoxSecondaryRole;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label labelFirstName;
        private System.Windows.Forms.Label labelLastName;
        private System.Windows.Forms.Label labelPrimaryRole;
        private System.Windows.Forms.Label labelSecondaryRole;
        private System.Windows.Forms.CheckBox checkBoxAvailable;
    }
}
