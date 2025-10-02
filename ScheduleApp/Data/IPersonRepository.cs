using System.Collections.Generic;
using GuardScheduler.Models;

namespace GuardScheduler.Data
{
    public interface IPersonRepository
    {
        List<Person> GetAll();
        Person GetById(int id);
        int Insert(Person p);
        void Update(Person p);
        void Delete(int id);
    }
}
