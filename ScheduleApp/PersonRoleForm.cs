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
                comboBoxPrimaryRole.Items.Add(TranslateRole(role));
                comboBoxSecondaryRole.Items.Add(TranslateRole(role));
            }

            comboBoxPrimaryRole.SelectedItem = TranslateRole(_person.PrimaryRole);
            comboBoxSecondaryRole.SelectedItem =
                _person.SecondaryRole.HasValue ? TranslateRole(_person.SecondaryRole.Value) : null;
        }

        private void LoadPersonData()
        {
            txtFirstName.Text = _person.FirstName;
            txtLastName.Text = _person.LastName;
            checkBoxAvailable.Checked = _person.Available;
            checkBoxMarried.Checked = _person.Married;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _person.FirstName = txtFirstName.Text.Trim();
            _person.LastName = txtLastName.Text.Trim();
            _person.PrimaryRole = ParseRole(comboBoxPrimaryRole.SelectedItem?.ToString());
            _person.SecondaryRole =  ParseRole(comboBoxSecondaryRole.SelectedItem.ToString());
            _person.Available = checkBoxAvailable.Checked;
            _person.Married = checkBoxMarried.Checked;

            _personRepository.Update(_person);
            MessageBox.Show("اطلاعات فرد با موفقیت ذخیره شد.", "ذخیره موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private string TranslateRole(Role role) =>
            role switch
        {
            Role.PasBakhsh => "پاس‌بخش",
            Role.Dezhban => "دژبان",
            Role.GoruhB => "گروه ب",
            Role.Ranandeh => "راننده",
            Role.KomakAshpaz => "کمک‌آشپز",
            Role.Negahban => "نگهبان",
            Role.AfsarGharargah => "افسر قرارگاه",
            Role.MohandesProject => "مهندس پروژه",
            Role.MoafAzRazm => "معاف از رزم",
            _ => role.ToString()
        };

        private Role ParseRole(string persianName) =>
            persianName switch
        {
            "پاس‌بخش" => Role.PasBakhsh,
            "دژبان" => Role.Dezhban,
            "گروه ب" => Role.GoruhB,
            "راننده" => Role.Ranandeh,
            "کمک‌آشپز" => Role.KomakAshpaz,
            "نگهبان" => Role.Negahban,
            "افسر قرارگاه" => Role.AfsarGharargah,
            "مهندس پروژه" => Role.MohandesProject,
            "معاف از رزم" => Role.MoafAzRazm,
            _ => Role.Negahban
        };
    }
}
