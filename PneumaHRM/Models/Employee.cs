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
            Field<StringGraphType>("key", resolve: x => x.Source.ADPrincipalName);
            Field<StringGraphType>("userName", resolve: x => x.Source.ADPrincipalName.Split('\\')[1]);

            AddNavigationListField(
                name: "balances",
                resolve: context => context.Source.Balances);

            Field<DecimalGraphType>("currentBalance",
                resolve: ctx => ctx.Source.Balances.Sum(x => x.Value));

        }
    }
}
