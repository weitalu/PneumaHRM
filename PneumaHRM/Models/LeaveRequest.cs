using GraphQL.DataLoader;
using GraphQL.EntityFramework;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class LeaveRequest : Entity
    {
        public int Id { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }

        public LeaveRequestState State { get; set; }
        public LeaveType Type { get; set; }

        public string RequestIssuerId { get; set; }
        public Employee RequestIssuer { get; set; }

        public List<LeaveRequestDeputy> Deputies { get; set; } = new List<LeaveRequestDeputy>();

        public List<LeaveRequestApprove> Approves { get; set; } = new List<LeaveRequestApprove>();

        public List<LeaveRequestComment> Comments { get; set; } = new List<LeaveRequestComment>();

        public List<RequestBalanceRelation> BalanceRelations { get; set; } = new List<RequestBalanceRelation>();


    }
    public class LeaveTypeEnum : EnumerationGraphType<LeaveType>
    {
    }
    public enum LeaveType
    {
        Annual,
        OverTime,
        Sick,
        Personal,
        Other,

    }
    public class LeaveRequestStateEnum : EnumerationGraphType<LeaveRequestState> { }
    public enum LeaveRequestState
    {
        New, Balanced,Completed
    }
    public class LeaveRequestInputType : InputObjectGraphType<LeaveRequest>
    {
        public LeaveRequestInputType()
        {
            Name = "LeaveRequestInput";
            Description = "A leave request";

            Field<NonNullGraphType<DateTimeGraphType>>()
                .Name("start")
                .Description("start time of the leave")
                .Resolve(ctx => ctx.Source.Start);
            Field<NonNullGraphType<DateTimeGraphType>>()
                .Name("end")
                .Description("end time of the leave")
                .Resolve(ctx => ctx.Source.End);
            Field<NonNullGraphType<LeaveTypeEnum>>()
                .Name("type")
                .Description("the type of the leave")
                .Resolve(ctx => ctx.Source.Type)
                .DefaultValue(LeaveType.Annual);
            Field<NonNullGraphType<StringGraphType>>()
                .Name("description")
                .Description("the description of the leave")
                .Resolve(ctx => ctx.Source.Description);
        }
    }
    public class LeaveRequestType : EfObjectGraphType<LeaveRequest>
    {
        public LeaveRequestType(IEfGraphQLService efGraphQLService) : base(efGraphQLService)
        {
            Field<IdGraphType>("Id", resolve: ctx => ctx.Source.Id);
            Field<StringGraphType>("name", resolve: ctx => ctx.Source.Name);
            Field<StringGraphType>("owner", resolve: ctx => ctx.Source.RequestIssuerId.Split('\\')[1]);
            Field<DateTimeGraphType>("from", resolve: ctx => ctx.Source.Start);
            Field<DateTimeGraphType>("to", resolve: ctx => ctx.Source.End);
            Field<LeaveTypeEnum>("type", resolve: ctx => ctx.Source.Type);
            Field<LeaveRequestStateEnum>("state", resolve: ctx => ctx.Source.State);
            Field<DecimalGraphType>("workHour",
                resolve: ctx =>
                {
                    var db = (ctx.UserContext as HrmContext).DbContext;
                    if (ctx.Source.End.HasValue && ctx.Source.Start.HasValue)
                    {
                        return db.Holidays
                        .Select(x => x.Value)
                        .ToList()
                        .GetWorkHours(ctx.Source.Start.Value, ctx.Source.End.Value);
                    }
                    else
                    {
                        return 0m;
                    }
                });
            AddNavigationListField(
               name: "deputies",
               resolve: context => context.Source.Deputies);
        }
    }
}
