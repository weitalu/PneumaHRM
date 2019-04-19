using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class Leave : Entity
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public DateTime ValidThru { get; set; }

        public LeaveType Type { get; set; }

        public int? OwnerId { get; set; }
        public Employee Owner { get; set; }
    }

}
