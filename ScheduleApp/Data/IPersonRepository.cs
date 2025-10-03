using System;
using System.Collections.Generic;
using GuardScheduler.Models;
public interface IPersonRepository
{
    List<Person> GetAll();
    Person GetById(int id);
    int Insert(Person person);
    void Update(Person person);
    void Delete(int id);

    event EventHandler<PersonChangedEventArgs> PersonChanged;
}

public class PersonChangedEventArgs : EventArgs
{
    public Person Person { get; set; }
    public PersonChangedEventArgs(Person person) { Person = person; }
}