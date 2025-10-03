using System;
using System.Windows.Forms;
using GuardScheduler.Models;
using GuardScheduler.Data;

namespace GuardScheduler
{
    public partial class PersonRoleForm : Form
    {
        private readonly IPersonRepository _personRepository;
        private Person _person;

        public PersonRoleForm(IPersonRepository personRepository, Person person)
        {
            InitializeComponent();
            _personRepository = personRepository;
            _person = person;

            LoadRoles();
            LoadPersonData();
        }

        private void LoadRoles()
        {
            comboBoxPrimaryRole.Items.Clear();
            comboBoxSecondaryRole.Items.Clear();

            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                comboBoxPrimaryRole.Items.Add(role);
                comboBoxSecondaryRole.Items.Add(role);
            }

            // Set current roles as default selection
            comboBoxPrimaryRole.SelectedItem = _person.PrimaryRole;
            if (_person.SecondaryRole.HasValue)
                comboBoxSecondaryRole.SelectedItem = _person.SecondaryRole.Value;
            else
                comboBoxSecondaryRole.SelectedIndex = -1;
        }

        private void LoadPersonData()
        {
            txtFirstName.Text = _person.FirstName;
            txtLastName.Text = _person.LastName;
            checkBoxAvailable.Checked = _person.Available; // Load availability
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _person.FirstName = txtFirstName.Text;
            _person.LastName = txtLastName.Text;
            _person.PrimaryRole = (Role)comboBoxPrimaryRole.SelectedItem;
            _person.SecondaryRole = comboBoxSecondaryRole.SelectedItem != null ? (Role?)comboBoxSecondaryRole.SelectedItem : null;
            _person.Available = checkBoxAvailable.Checked; // Save availability

            _personRepository.Update(_person);

            MessageBox.Show("Person roles and availability updated successfully!");
            this.Close();
        }
    }
}
