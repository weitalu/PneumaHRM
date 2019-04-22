using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class Employee : Entity
    {
        public int Id { get; set; }
        public string ADPrincipalName { get; set; }
        public bool isActive { get; set; }
        public DateTime? OnboardDate { get; set; }

        public List<LeaveBalance> Balances { get; set; }
        public List<LeaveRequest> Leaves { get; set; }
        public List<LeaveRequestApprove> Approves { get; set; }
        public List<LeaveRequestDeputy> Deputies { get; set; }
        public List<LeaveRequestComment> RequestComments { get; set; }
    }
}
