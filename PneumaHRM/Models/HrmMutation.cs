using GraphQL.Builders;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

            Field<LeaveRequestType, LeaveRequest>()
                 .Name("createLeaveRequest")
                 .Argument<NonNullGraphType<LeaveRequestInputType>>("input", "The leave request you want to create.")
                 .BuildMutationResolver<LeaveRequest, LeaveRequest>(HRMUtility.CreateLeaveRequest);

            Field<StringGraphType, string>()
                .Name("deleteLeaveRequest")
                .Argument<NonNullGraphType<IdGraphType>>("input", "The leave request id  you want to delete")
                .BuildMutationResolver<string, int>(HRMUtility.DeleteLeaveRequest);

            Field<LeaveRequestType, LeaveRequest>()
                 .Name("completeLeaveRequest")
                 .Argument<NonNullGraphType<IdGraphType>>("input", "The Id of the target leave request to be complete")
                 .BuildMutationResolver<LeaveRequest, int>(HRMUtility.CompleteLeaveRequest);

            Field<LeaveBalanceType, LeaveBalance>()
                .Name("createLeaveBalance")
                .Argument<NonNullGraphType<LeaveBalanceInputType>>("input", "The balance you want to create")
                .BuildMutationResolver<LeaveBalance, LeaveBalance>(HRMUtility.CreateLeaveBalance);

            Field<LeaveRequestType, LeaveRequest>()
                .Name("approveLeaveRequest")
                .Argument<NonNullGraphType<IntGraphType>>("leaveRequestId", "The Id of the target leave request to be approved")
                .Argument<StringGraphType>("comment", "the comment")
                .Resolve(ctx =>
                {
                    var db = (ctx.UserContext as HrmContext).DbContext;
                    var userName = (ctx.UserContext as HrmContext).UserContext.Identity.Name;
                    var targetId = ctx.GetArgument<int>("leaveRequestId");
                    var comment = ctx.GetArgument<string>("comment");
                    var target = db.LeaveRequests.Find(targetId);
                    var canDeput = target.CanApproveBy(userName);
                    if (!canDeput.able) throw new GraphQL.ExecutionError(canDeput.reason);
                    target.Approve(comment);
                    db.SaveChanges();
                    return target;
                });
            Field<LeaveRequestType, LeaveRequest>()
                .Name("deputyLeaveRequest")
                .Argument<NonNullGraphType<IntGraphType>>("leaveRequestId", "The Id of the target leave request to be approved")
                .Argument<StringGraphType>("comment", "the comment")
                .Resolve(ctx =>
                {
                    var db = (ctx.UserContext as HrmContext).DbContext;
                    var userName = (ctx.UserContext as HrmContext).UserContext.Identity.Name;
                    var targetId = ctx.GetArgument<int>("leaveRequestId");
                    var comment = ctx.GetArgument<string>("comment");
                    var target = db.LeaveRequests.Find(targetId);
                    var canDeput = target.CanDeputyBy(userName);
                    if (!canDeput.able) throw new GraphQL.ExecutionError(canDeput.reason);
                    target.Deputy(comment);
                    db.SaveChanges();
                    return target;
                });
        }
    }
}
