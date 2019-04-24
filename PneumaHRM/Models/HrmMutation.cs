using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class HrmMutation : ObjectGraphType
    {
        public HrmMutation()
        {
            Field<LeaveRequestType>(
                "createLeaveRequest",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<LeaveRequestInputType>>()
                    {
                        Name = "leaveRequest",
                        Description = "The leave request you want to create.",
                    }),
                resolve: ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var db = hrmCtx.DbContext;
                    var userName = hrmCtx.UserContext.Identity.Name;
                    var userId = db.Employees.Where(x => x.ADPrincipalName == userName).FirstOrDefault().Id;
                    var leaveRequest = ctx.GetArgument<LeaveRequest>("leaveRequest");
                    leaveRequest.RequestIssuerId = userId;
                    db.LeaveRequests.Add(leaveRequest);
                    return leaveRequest;
                });

        }
    }
}
