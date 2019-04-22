using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class LeaveRequest : Entity
    {
        public int Id { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }

        public LeaveRequestState State { get; set; }
        public LeaveType Type { get; set; }

        public int? RequestIssuerId { get; set; }
        public Employee RequestIssuer { get; set; }

        public List<LeaveRequestDeputy> Deputies { get; set; }

        public List<LeaveRequestApprove> Approves { get; set; }

        public List<LeaveRequestComment> Comments { get; set; }


    }

    public enum LeaveType
    {
        Annual, OverTime, Sick, Personal, Other
    }
    public enum LeaveRequestState
    {
    }
}
