namespace GuardScheduler
{
    partial class PersonRoleForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.ComboBox comboBoxPrimaryRole;
        private System.Windows.Forms.ComboBox comboBoxSecondaryRole;
        private System.Windows.Forms.CheckBox checkBoxAvailable;
        private System.Windows.Forms.CheckBox checkBoxMarried;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label labelFirstName;
        private System.Windows.Forms.Label labelLastName;
        private System.Windows.Forms.Label labelPrimaryRole;
        private System.Windows.Forms.Label labelSecondaryRole;
        private System.Windows.Forms.Label labelAvailable;
        private System.Windows.Forms.Label labelMarried;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.comboBoxPrimaryRole = new System.Windows.Forms.ComboBox();
            this.comboBoxSecondaryRole = new System.Windows.Forms.ComboBox();
            this.checkBoxAvailable = new System.Windows.Forms.CheckBox();
            this.checkBoxMarried = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.labelFirstName = new System.Windows.Forms.Label();
            this.labelLastName = new System.Windows.Forms.Label();
            this.labelPrimaryRole = new System.Windows.Forms.Label();
            this.labelSecondaryRole = new System.Windows.Forms.Label();
            this.labelAvailable = new System.Windows.Forms.Label();
            this.labelMarried = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // 
            // txtFirstName
            // 
            this.txtFirstName.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtFirstName.Location = new System.Drawing.Point(60, 40);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(250, 24);
            this.txtFirstName.TabIndex = 0;
            // 
            // txtLastName
            // 
            this.txtLastName.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtLastName.Location = new System.Drawing.Point(60, 90);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(250, 24);
            this.txtLastName.TabIndex = 1;
            // 
            // comboBoxPrimaryRole
            // 
            this.comboBoxPrimaryRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPrimaryRole.Font = new System.Drawing.Font("Tahoma", 10F);
            this.comboBoxPrimaryRole.Location = new System.Drawing.Point(60, 140);
            this.comboBoxPrimaryRole.Name = "comboBoxPrimaryRole";
            this.comboBoxPrimaryRole.Size = new System.Drawing.Size(250, 24);
            this.comboBoxPrimaryRole.TabIndex = 2;
            // 
            // comboBoxSecondaryRole
            // 
            this.comboBoxSecondaryRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSecondaryRole.Font = new System.Drawing.Font("Tahoma", 10F);
            this.comboBoxSecondaryRole.Location = new System.Drawing.Point(60, 190);
            this.comboBoxSecondaryRole.Name = "comboBoxSecondaryRole";
            this.comboBoxSecondaryRole.Size = new System.Drawing.Size(250, 24);
            this.comboBoxSecondaryRole.TabIndex = 3;
            // 
            // checkBoxAvailable
            // 
            this.checkBoxAvailable.AutoSize = true;
            this.checkBoxAvailable.Font = new System.Drawing.Font("Tahoma", 10F);
            this.checkBoxAvailable.Location = new System.Drawing.Point(60, 240);
            this.checkBoxAvailable.Name = "checkBoxAvailable";
            this.checkBoxAvailable.Size = new System.Drawing.Size(88, 21);
            this.checkBoxAvailable.TabIndex = 4;
            this.checkBoxAvailable.Text = "در دسترس";
            this.checkBoxAvailable.UseVisualStyleBackColor = true;
            // 
            // checkBoxMarried
            // 
            this.checkBoxMarried.AutoSize = true;
            this.checkBoxMarried.Font = new System.Drawing.Font("Tahoma", 10F);
            this.checkBoxMarried.Location = new System.Drawing.Point(180, 240);
            this.checkBoxMarried.Name = "checkBoxMarried";
            this.checkBoxMarried.Size = new System.Drawing.Size(71, 21);
            this.checkBoxMarried.TabIndex = 5;
            this.checkBoxMarried.Text = "متأهل";
            this.checkBoxMarried.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(60, 290);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(250, 40);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "ذخیره اطلاعات";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Labels
            // 
            this.labelFirstName.AutoSize = true;
            this.labelFirstName.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelFirstName.Location = new System.Drawing.Point(330, 43);
            this.labelFirstName.Text = "نام:";
            this.labelLastName.AutoSize = true;
            this.labelLastName.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelLastName.Location = new System.Drawing.Point(330, 93);
            this.labelLastName.Text = "نام خانوادگی:";
            this.labelPrimaryRole.AutoSize = true;
            this.labelPrimaryRole.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelPrimaryRole.Location = new System.Drawing.Point(330, 143);
            this.labelPrimaryRole.Text = "نقش اصلی:";
            this.labelSecondaryRole.AutoSize = true;
            this.labelSecondaryRole.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelSecondaryRole.Location = new System.Drawing.Point(330, 193);
            this.labelSecondaryRole.Text = "نقش دوم:";
            // 
            // PersonRoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.ClientSize = new System.Drawing.Size(420, 370);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.checkBoxMarried);
            this.Controls.Add(this.checkBoxAvailable);
            this.Controls.Add(this.comboBoxSecondaryRole);
            this.Controls.Add(this.comboBoxPrimaryRole);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.labelSecondaryRole);
            this.Controls.Add(this.labelPrimaryRole);
            this.Controls.Add(this.labelLastName);
            this.Controls.Add(this.labelFirstName);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.Name = "PersonRoleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "ویرایش اطلاعات فرد";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
