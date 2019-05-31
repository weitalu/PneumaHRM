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
            Field<LeaveRequestType>()
                .Name("createLeaveRequest")
                .Argument<NonNullGraphType<LeaveRequestInputType>>("leaveRequest", "The leave request you want to create.")
                .Resolve(ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var db = hrmCtx.DbContext;
                    var userName = hrmCtx.UserContext.Identity.Name;
                    var leaveRequest = ctx.GetArgument<LeaveRequest>("leaveRequest");
                    leaveRequest.RequestIssuerId = userName;
                    db.LeaveRequests.Add(leaveRequest);
                    db.SaveChanges();
                    return leaveRequest;
                });
            Field<StringGraphType>()
                .Name("deleteLeaveRequest")
                .Argument<NonNullGraphType<IdGraphType>>("requestId", "")
                .Resolve(ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var db = hrmCtx.DbContext;
                    var requestId = ctx.GetArgument<int>("requestId");
                    var request = db.LeaveRequests.Find(requestId);
                    if (!request.CanDelete()) throw new Exception($"{request.State.ToString()} can't not be deleted");
                    db.LeaveRequests.Remove(request);
                    db.SaveChanges();
                    return "success";
                });
            Field<LeaveBalanceType>()
                 .Name("completeLeaveRequest")
                 .Argument<NonNullGraphType<IdGraphType>>("requestId", "The Id of the target leave request to be balanced")
                 .Argument<DecimalGraphType>("balanceHour", "")
                 .Argument<StringGraphType>("description", "")
                 .Resolve(ctx =>
                 {
                     var db = (ctx.UserContext as HrmContext).DbContext;
                     var requestId = ctx.GetArgument<int>("requestId");
                     var balanceHour = ctx.GetArgument<decimal>("balanceHour");
                     var description = ctx.GetArgument<string>("description");
                     var leaveRequst = db.LeaveRequests.Find(requestId);
                     if (!leaveRequst.CanBalance())
                         throw new Exception($"the leave request can't be balanced");


                     var balance = new LeaveBalance()
                     {
                         Value = balanceHour,
                         OwnerId = leaveRequst.RequestIssuerId,
                         Description = description,
                         SnapShotData = JsonConvert.SerializeObject(new
                         {
                             requestId = leaveRequst.Id,
                             requestFrom = leaveRequst.Start,
                             requestTo = leaveRequst.End,
                             type = leaveRequst.Type.ToString(),
                             approves = leaveRequst.Comments
                                     .Select(x => new
                                     {
                                         x.Content,
                                         x.CreatedOn,
                                         x.CreatedBy,
                                         Type = x.Type.ToString()
                                     })
                                     .ToList(),

                         })
                     };
                     balance.RequestRelations.Add(new RequestBalanceRelation()
                     {
                         Balance = balance,
                         RequestId = requestId
                     });
                     db.LeaveBalances.Add(balance);
                     leaveRequst.State = LeaveRequestState.Completed;
                     db.SaveChanges();
                     return balance;
                 });
            Field<LeaveBalanceType>()
                .Name("createLeaveBalance")
                .Argument<NonNullGraphType<LeaveBalanceInputType>>("leaveBalance", "")
                .Resolve(ctx =>
                {
                    var db = (ctx.UserContext as HrmContext).DbContext;
                    var balance = ctx.GetArgument<LeaveBalance>("leaveBalance");
                    db.LeaveBalances.Add(balance);
                    db.SaveChanges();
                    return balance;
                });
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
