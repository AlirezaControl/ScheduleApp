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

            LoadPersonData();
            LoadRoles();
        }

        private void LoadPersonData()
        {
            txtFirstName.Text = _person.FirstName;
            txtLastName.Text = _person.LastName;
            comboBoxPrimaryRole.SelectedItem = _person.PrimaryRole.ToString();
            comboBoxSecondaryRole.SelectedItem = _person.SecondaryRole?.ToString();
            checkBoxAvailable.Checked = _person.Available; // Load availability
        }

        private void LoadRoles()
        {
            foreach (var role in Enum.GetValues(typeof(Role)))
            {
                comboBoxPrimaryRole.Items.Add(role);
                comboBoxSecondaryRole.Items.Add(role);
            }
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
