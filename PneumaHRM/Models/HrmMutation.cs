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
                    var leaveRequest = ctx.GetArgument<LeaveRequest>("leaveRequest");
                    leaveRequest.RequestIssuerId = userName;
                    db.LeaveRequests.Add(leaveRequest);
                    db.SaveChanges();
                    return leaveRequest;
                });
            Field<StringGraphType>(
                "deleteLeaveRequest",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>()
                    {
                        Name = "requestId"
                    }),
                resolve: ctx =>
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

            Field<LeaveBalanceType>(
                "balanceLeaveRequest",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>()
                    {
                        Name = "requestId",
                        Description = "The Id of the target leave request to be balanced"
                    },
                    new QueryArgument<NonNullGraphType<DecimalGraphType>>()
                    {
                        Name = "balanceHour"
                    },
                    new QueryArgument<NonNullGraphType<StringGraphType>>()
                    {
                        Name = "description"
                    }),
                    resolve: ctx =>
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
            Field<LeaveBalanceType>(
                "createLeaveBalance",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType>()
                    {
                        Name = "userName"
                    },
                    new QueryArgument<NonNullGraphType<DecimalGraphType>>()
                    {
                        Name = "balanceHour"
                    },
                    new QueryArgument<NonNullGraphType<StringGraphType>>()
                    {
                        Name = "description"
                    }),
                resolve: ctx =>
                    {
                        var hrmCtx = ctx.UserContext as HrmContext;
                        var db = hrmCtx.DbContext;
                        var userName = ctx.GetArgument<string>("userName") ?? hrmCtx.UserContext.Identity.Name;
                        var balanceHour = ctx.GetArgument<decimal>("balanceHour");
                        var description = ctx.GetArgument<string>("description");
                        var balance = new LeaveBalance()
                        {
                            Value = balanceHour,
                            OwnerId = userName,
                            Description = description,
                        };
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
