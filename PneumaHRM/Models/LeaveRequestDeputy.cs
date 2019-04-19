using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class LeaveRequestDeputy : Entity
    {
        public int Id { get; set; }
        public int Order { get; set; }

        public int RequestId { get; set; }
        public LeaveRequest Request { get; set; }

        public int? DeputyById { get; set; }
        public Employee DeputyBy { get; set; }
    }
}
