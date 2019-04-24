using GraphQL.Types;
using GraphQL.Types.Relay;
using GraphQL.Types.Relay.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class HrmQuery : ObjectGraphType
    {
        public HrmQuery()
        {
            Field<ListGraphType<HolidayType>>(
                "holidays",
                 arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<DateTimeGraphType>> { Name = "from" },
                    new QueryArgument<NonNullGraphType<DateTimeGraphType>> { Name = "to" }
                ),
                resolve: ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var db = hrmCtx.DbContext;
                    var from = ctx.GetArgument<DateTime>("from").Date;
                    var to = ctx.GetArgument<DateTime>("to").Date;
                    return db.Holidays.Where(x => x.Value.Date >= from && x.Value.Date <= to).ToList();
                });

            Field<ListGraphType<EmployeeType>>(
                "employees",
                resolve: ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var db = hrmCtx.DbContext;
                    return db.Employees.ToList();
                });
            Field<DecimalGraphType>(
                "workHours",
                 arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<DateTimeGraphType>> { Name = "from" },
                    new QueryArgument<NonNullGraphType<DateTimeGraphType>> { Name = "to" }
                ),
                resolve: ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var db = hrmCtx.DbContext;
                    var from = ctx.GetArgument<DateTime>("from");
                    var to = ctx.GetArgument<DateTime>("to");
                    return db.Holidays
                        .Select(x => x.Value)
                        .ToList()
                        .GetWorkHours(from, to);
                });
            Field<EmployeeType>(
                "self",
                resolve: ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var db = hrmCtx.DbContext;
                    var userName = hrmCtx.UserContext.Identity.Name;
                    return db.Employees.Where(x => x.ADPrincipalName == userName).FirstOrDefault();
                });
        }
    }
}
