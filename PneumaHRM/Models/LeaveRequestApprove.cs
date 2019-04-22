using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class LeaveRequestApprove : Entity
    {
        public int Id { get; set; }

        public int? RequestId { get; set; }
        public LeaveRequest Request { get; set; }

        public int? ApproveById { get; set; }
        public Employee ApproveBy { get; set; }
    }
}
