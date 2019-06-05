using GraphQL.Builders;
using GraphQL.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public static class HRMUtility
    {
        public static FieldBuilder<object, TReturnType> BuildMutationResolver<TReturnType, TArg>(
            this FieldBuilder<object, TReturnType> builder,
            Func<HrmContext, TArg, TReturnType> func)
        {
            return builder.Resolve(x =>
           {
               var arg = x.GetArgument<TArg>("input");
               var hrmCtx = x.UserContext as HrmContext;
               var result = func(hrmCtx, arg);
               hrmCtx.DbContext.SaveChanges();
               return result;
           });
        }

        public static string DeleteLeaveRequest(HrmContext ctx, int id)
        {
            var request = ctx.DbContext.LeaveRequests.Find(id);
            if (!request.CanDelete()) throw new Exception($"{request.State.ToString()} can't not be deleted");
            ctx.DbContext.LeaveRequests.Remove(request);
            return "success";
        }
        public static LeaveRequest CreateLeaveRequest(HrmContext ctx, LeaveRequest request)
        {
            request.RequestIssuerId = ctx.UserContext.Identity.Name;
            ctx.DbContext.LeaveRequests.Add(request);
            return request;
        }
        public static LeaveRequest CompleteLeaveRequest(HrmContext ctx, int id)
        {
            var leaveRequst = ctx.DbContext.LeaveRequests.Find(id);
            if (!leaveRequst.CanBalance())
                throw new Exception($"the leave request can't be balanced");


            var balance = new LeaveBalance()
            {
                Value = -ctx.Holidays.GetWorkHours(leaveRequst.Start.Value, leaveRequst.End.Value),
                OwnerId = leaveRequst.RequestIssuerId,
                Description = "A completed Leave Request",
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
                RequestId = id
            });
            ctx.DbContext.LeaveBalances.Add(balance);
            ctx.DbContext.LeaveRequestComments.Add(new LeaveRequestComment()
            {
                Content = "Completed",
                Type = CommentType.None,
                RequestId = leaveRequst.Id
            });
            leaveRequst.State = LeaveRequestState.Completed;
            return leaveRequst;
        }
        public static LeaveBalance CreateLeaveBalance(HrmContext ctx, LeaveBalance balance)
        {
            ctx.DbContext.Add(balance);
            return balance;
        }
        public static LeaveRequest ApproveLeaveRequest(HrmContext ctx, LeaveRequestComment comment)
        {
            var target = ctx.DbContext.LeaveRequests.Find(comment.RequestId);
            var canApprove = target.CanApproveBy(ctx.UserContext.Identity.Name);
            if (!canApprove.able) throw new GraphQL.ExecutionError(canApprove.reason);
            comment.Type = CommentType.Approve;
            target.State = LeaveRequestState.Processing;
            ctx.DbContext.Add(comment);
            return target;
        }
        public static LeaveRequest DeputyLeaveRequest(HrmContext ctx, LeaveRequestComment comment)
        {
            var target = ctx.DbContext.LeaveRequests.Find(comment.RequestId);
            var canDeput = target.CanDeputyBy(ctx.UserContext.Identity.Name);
            if (!canDeput.able) throw new GraphQL.ExecutionError(canDeput.reason);
            comment.Type = CommentType.Deputy;
            target.State = LeaveRequestState.Processing;
            ctx.DbContext.Add(comment);
            return target;
        }
        public static LeaveRequest CommentLeaveRequest(HrmContext ctx, LeaveRequestComment comment)
        {
            var target = ctx.DbContext.LeaveRequests.Find(comment.RequestId);
            if (target == null) throw new GraphQL.ExecutionError("request not exists");
            comment.Type = CommentType.None;
            ctx.DbContext.Add(comment);
            return target;
        }
        public static (bool able, string reason) CanApproveBy(this LeaveRequest target, string approver)
        {
            if (target == null) return (false, "target not exist");
            return (true, "");
        }
        public static (bool able, string reason) CanDeputyBy(this LeaveRequest target, string deputy)
        {
            if (target == null) return (false, "target not exist");
            return (true, "");
        }
        public static bool CanDelete(this LeaveRequest target)
        {
            return target != null && (target.State != LeaveRequestState.Completed);
        }
        public static bool CanBalance(this LeaveRequest target)
        {
            return target != null && (target.State == LeaveRequestState.Completed);
        }

        public static decimal GetWorkHours(this List<DateTime> holidays, DateTime start, DateTime end)
        {
            decimal result = 0m;
            for (var current = start; current < end; current = current.AddMinutes(30))
            {
                var isWorkHour = !holidays.Contains(current.Date) && current.Hour >= 9 && current.Hour < 18 && current.Hour != 12;
                if (isWorkHour) result += 0.5m;
            }
            return result;
        }
        public static int ImportHoliday(this HrmDbContext db, List<HolidayDTO> data)
        {
            var holidays = db.Holidays.Select(x => x.Value.Date).ToList();
            var toImport = data.Where(x => x.IsHoliday == "是")
                .Where(x => !holidays.Contains(x.Date.Date))
                .Select(x => new Holiday()
                {
                    Name = x.Name,
                    Description = $"{x.HolidayCategory}. {x.Description}".Trim(),
                    Value = x.Date
                }).ToList();

            db.Holidays.AddRange(toImport);
            return db.SaveChanges();
        }
        public static void SeedData(this HrmDbContext db)
        {
            db.Database.EnsureCreated();
            if (db.Holidays.Count() == 0)
            {
                var json = File.ReadAllText("InitData/Holidays.json");
                var data = JArray.Parse(json).ToObject<List<HolidayDTO>>();
                db.ImportHoliday(data);
            }
        }
    }
}
