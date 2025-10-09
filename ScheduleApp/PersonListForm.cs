using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GuardScheduler.Models;
using GuardScheduler.Data;

namespace GuardScheduler
{
    public partial class PersonListForm : Form
    {
        private readonly IPersonRepository _personRepository;

        public PersonListForm(IPersonRepository personRepository)
        {
            InitializeComponent();
            _personRepository = personRepository;
            LoadPersons();
        }

        private void LoadPersons()
        {
            dataGridViewPersons.Rows.Clear();
            List<Person> persons = _personRepository.GetAll();

            foreach (var person in persons)
            {
                string primaryRole = TranslateRole(person.PrimaryRole.ToString());
                string secondaryRole = person.SecondaryRole.HasValue
                    ? TranslateRole(person.SecondaryRole.Value.ToString())
                    : "-";

                string marriedText = person.Married ? "متأهل" : "مجرد";
                string availableText = person.Available ? "در دسترس" : "غیرفعال";

                dataGridViewPersons.Rows.Add(
                    $"{person.FirstName} {person.LastName}",
                    primaryRole,
                    secondaryRole,
                    marriedText,
                    availableText,
                    person.RotationOrder
                );
            }
        }

        private string TranslateRole(string roleName)
        {
            // Converts enum identifiers to readable Persian
            return roleName switch
            {
                "PasBakhsh" => "پاس‌بخش",
                "Dezhban" => "دژبان",
                "GoruhB" => "گروه ب",
                "Ranandeh" => "راننده",
                "KomakAshpaz" => "کمک‌آشپز",
                "Negahban" => "نگهبان",
                "AfsarGharargah" => "افسر قرارگاه",
                "MohandesProject" => "مهندس پروژه",
                "MoafAzRazm" => "معاف از رزم",
                _ => roleName
            };
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPersons();
        }

        private void dataGridViewPersons_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _personRepository.GetAll().Count)
            {
                var person = _personRepository.GetAll()[e.RowIndex];
                var form = new PersonRoleForm(_personRepository, person);
                form.ShowDialog();
                LoadPersons();
            }
        }
    }
}
