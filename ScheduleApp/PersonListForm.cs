using System;
using System.Collections.Generic;
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
            listBoxPersons.Items.Clear();
            List<Person> persons = _personRepository.GetAll();
            foreach (var person in persons)
            {
                listBoxPersons.Items.Add(person);
            }
        }

        private void listBoxPersons_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPersons.SelectedItem is Person selectedPerson)
            {
                var form = new PersonRoleForm(_personRepository, selectedPerson);
                form.ShowDialog();
                LoadPersons(); // Refresh the list after closing the form
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPersons(); // Refresh the list when the button is clicked
        }
    }
}