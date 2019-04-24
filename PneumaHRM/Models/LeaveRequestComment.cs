using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class LeaveRequestComment : Entity
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public int? RequestId { get; set; }
        public LeaveRequest Request { get; set; }

        public string CommentBy { get; set; }
    }
}
