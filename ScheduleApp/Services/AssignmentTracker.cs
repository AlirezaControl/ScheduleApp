using System.Collections.Generic;
using System.Linq;
using GuardScheduler.Models;

namespace GuardScheduler.Services
{
    public class AssignmentTracker
    {
        private readonly IPersonRepository _personRepo;
        private readonly Dictionary<int, int> _assignmentsThisWeek = new Dictionary<int, int>();

        public AssignmentTracker(IPersonRepository personRepo)
        {
            _personRepo = personRepo;
            InitAssignments();
        }

        private void InitAssignments()
        {
            var people = _personRepo.GetAll().Where(p => p.Available);
            foreach (var p in people)
                _assignmentsThisWeek[p.Id] = 0;
        }

        public void ResetWeeklyAssignments()
        {
            var keys = _assignmentsThisWeek.Keys.ToList();
            foreach (var key in keys)
                _assignmentsThisWeek[key] = 0;
        }

        public bool CanAssign(Person person)
        {
            if (!person.Available) return false;
            if (person.Married && _assignmentsThisWeek.TryGetValue(person.Id, out int count) && count >= 1)
                return false;
            return true;
        }

        public void RecordAssignment(int personId)
        {
            if (!_assignmentsThisWeek.ContainsKey(personId))
                _assignmentsThisWeek[personId] = 0;
            _assignmentsThisWeek[personId]++;
        }
    }
}
