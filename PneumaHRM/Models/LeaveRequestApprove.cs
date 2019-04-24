using GraphQL.Types;
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

        public string ApproveBy { get; set; }
    }

    public class LeaveRequestApproveType : ObjectGraphType<LeaveRequestApprove>
    {
    }
}
