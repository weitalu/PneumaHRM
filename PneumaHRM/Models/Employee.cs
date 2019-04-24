using GraphQL.Types;
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

    public class EmployeeType : ObjectGraphType<Employee>
    {
        public EmployeeType()
        {
            Field(x => x.ADPrincipalName).Name("UserName");
            Field<ListGraphType<LeaveBalanceType>>("leaveBalances",
              resolve: context =>
              {
                  var db = (context.UserContext as HrmContext).DbContext;
                  var employeeId = context.Source.Id;
                  return db.LeaveBalances.Where(x => x.OwnerId == employeeId).ToList();
              },
              description: "your leave balance history");

            Field<ListGraphType<LeaveRequestType>>("leaveRequests",
              resolve: context =>
              {
                  var db = (context.UserContext as HrmContext).DbContext;
                  var employeeId = context.Source.Id;
                  return db.LeaveRequests.Where(x => x.RequestIssuerId == employeeId).ToList();
              },
              description: "your current leave requests");

        }
    }
}
