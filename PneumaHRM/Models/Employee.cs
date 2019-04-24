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
                  var userName = (context.UserContext as HrmContext).UserContext.Identity.Name;
                  return db.LeaveBalances.Where(x => x.Owner.ADPrincipalName == userName).ToList();
              },
              description: "your leave balance history");

            Field<ListGraphType<LeaveRequestType>>("leaveRequests",
              resolve: context =>
              {
                  var db = (context.UserContext as HrmContext).DbContext;
                  var userName = (context.UserContext as HrmContext).UserContext.Identity.Name;
                  return db.LeaveRequests.Where(x => x.RequestIssuer.ADPrincipalName == userName).ToList();
              },
              description: "your current leave requests");

        }
    }
}
