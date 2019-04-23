using GraphQL.Types;
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
                resolve: ctx =>
                {
                    using (var db = new HrmDbContext())
                    {
                        return db.Holidays.ToList();
                    }
                });

            Field<ListGraphType<EmployeeType>>(
                "employees",
                resolve: ctx =>
                {
                    using (var db = new HrmDbContext())
                    {
                        return db.Employees.ToList();
                    };
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
