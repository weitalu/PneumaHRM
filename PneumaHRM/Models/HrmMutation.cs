using GraphQL.Types;
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
                    if (request.State != LeaveRequestState.New) throw new Exception($"{request.State.ToString()} can't not be deleted");
                    db.LeaveRequests.Remove(request);
                    db.SaveChanges();
                    return "success";
                });

            Field<LeaveBalanceType>(
                "balanceLeaveRequest",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>()
                    {
                        Name = "leaveRequestId",
                        Description = "The Id of the target leave request to be balanced"
                    }),
                resolve: ctx =>
                {
                    var db = (ctx.UserContext as HrmContext).DbContext;
                    var targetId = ctx.GetArgument<int>("leaveRequestId");
                    var leaveRequst = db.LeaveRequests.Find(targetId);
                    if (leaveRequst.State != LeaveRequestState.New)
                        throw new Exception($"the leave request can't be balanced ({leaveRequst.State.ToString()})");
                    var holidays = db.Holidays.Select(x => x.Value).ToList();
                    var balance = new LeaveBalance()
                    {
                        Value = -holidays.GetWorkHours(leaveRequst.Start.Value, leaveRequst.End.Value),
                        OwnerId = leaveRequst.RequestIssuerId,
                        Description = "The leave request balance",
                        SnapShotData = JsonConvert.SerializeObject(new
                        {
                            requestId = leaveRequst.Id,
                            requestFrom = leaveRequst.Start,
                            requestTo = leaveRequst.End,
                            type = leaveRequst.Type.ToString(),
                            approves = db.LeaveRequestApproves
                                        .Where(x => x.RequestId == targetId)
                                        .Select(x => x.ApproveBy)
                                        .ToList(),
                        })
                    };
                    db.LeaveBalances.Add(balance);
                    leaveRequst.State = LeaveRequestState.Balanced;
                    return balance;
                });

            Field<StringGraphType>(
                "approveLeaveRequest",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>>()
                    {
                        Name = "leaveRequestId",
                        Description = "The Id of the target leave request to be approved"
                    }),
                resolve: ctx =>
                {
                    var db = (ctx.UserContext as HrmContext).DbContext;
                    var userName = (ctx.UserContext as HrmContext).UserContext.Identity.Name;
                    var targetId = ctx.GetArgument<int>("leaveRequestId");
                    var target = db.LeaveRequests.Find(targetId);
                    if (target == null) return "target request not exists";
                    if (db.LeaveRequestApproves.Where(x => x.RequestId == targetId && x.ApproveBy == userName).Count() == 0)
                    {
                        db.LeaveRequestApproves.Add(new LeaveRequestApprove()
                        {
                            ApproveBy = userName,
                            RequestId = targetId,
                        });
                    }
                    return "success";
                });

        }
    }
}
