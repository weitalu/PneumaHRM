using GraphQL.EntityFramework;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class Employee : Entity
    {
        public string ADPrincipalName { get; set; }
        public bool isActive { get; set; }
        public DateTime? OnboardDate { get; set; }

        public List<LeaveBalance> Balances { get; set; } = new List<LeaveBalance>();
        public List<LeaveRequest> Leaves { get; set; } = new List<LeaveRequest>();
    }

    public class EmployeeType : EfObjectGraphType<Employee>
    {
        public EmployeeType(IEfGraphQLService graphQlService) :
            base(graphQlService)
        {
            Field<StringGraphType>("userName", resolve: x => x.Source.ADPrincipalName.Split('\\')[1]);
            AddNavigationListField(
                name: "balances",
                resolve: context => context.Source.Balances);
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
