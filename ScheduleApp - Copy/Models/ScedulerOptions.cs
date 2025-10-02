using System;
using System.Collections.Generic;

namespace GuardScheduler.Models
{
    public class SchedulerOptions
    {
        public Dictionary<Role, int> DefaultShiftHours { get; set; } = new Dictionary<Role, int>
        {
            { Role.PasBakhsh, 4 },
            { Role.Dezhban, 2 },
            { Role.Negahban, 2 },
            { Role.GoruhB, 8 },
            { Role.Ranandeh, 8 },
            { Role.KomakAshpaz, 8 },
            { Role.AfsarGharargah, 8 },
            { Role.MohandesProject, 8 }
        };

        public List<Role> ReadyForceRoles { get; set; } = new List<Role> { Role.PasBakhsh, Role.Dezhban };

        public Func<Role, bool> RestNextDayApplies { get; set; } = role => role != Role.GoruhB;
    }
}
