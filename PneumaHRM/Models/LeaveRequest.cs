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
        New, Processing, Completed
    }
    public class LeaveRequestInputType : InputObjectGraphType<LeaveRequest>
    {
        public LeaveRequestInputType()
        {
            Name = "LeaveRequestInput";
            Description = "A leave request";

            Field<NonNullGraphType<DateTimeGraphType>>()
                .Name("start")
                .Description("start time of the leave");
            Field<NonNullGraphType<DateTimeGraphType>>()
                .Name("end")
                .Description("end time of the leave");
            Field<NonNullGraphType<LeaveTypeEnum>>()
                .Name("type")
                .Description("the type of the leave")
                .DefaultValue(LeaveType.Annual);
            Field<NonNullGraphType<StringGraphType>>()
                .Name("description")
                .Description("the description of the leave");
        }
    }
    public class LeaveRequestType : EfObjectGraphType<LeaveRequest>
    {
        public LeaveRequestType(IEfGraphQLService efGraphQLService) : base(efGraphQLService)
        {
            Field<IdGraphType>("Id", resolve: ctx => ctx.Source.Id);
            Field<StringGraphType>("name", resolve: ctx => ctx.Source.Name);
            Field<DateTimeGraphType>("createdOn", resolve: ctx => ctx.Source.CreatedOn);
            Field<StringGraphType>("owner", resolve: ctx => ctx.Source.RequestIssuerId.Split('\\')[1]);
            Field<DateTimeGraphType>("from", resolve: ctx => ctx.Source.Start);
            Field<DateTimeGraphType>("to", resolve: ctx => ctx.Source.End);
            Field<LeaveTypeEnum>("type", resolve: ctx => ctx.Source.Type);
            Field<LeaveRequestStateEnum>("state", resolve: ctx => ctx.Source.State);
            Field<StringGraphType>("description", resolve: ctx => ctx.Source.Description);
            Field<BooleanGraphType>("canDelete", resolve: ctx => ctx.Source.CanDelete());
            Field<BooleanGraphType>("canApproveBy", resolve: ctx => ctx.Source.CanApproveBy("").able);
            Field<BooleanGraphType>("canDeputyBy", resolve: ctx =>
            {
                var username = (ctx.UserContext as HrmContext).UserContext.Identity.Name;
                return ctx.Source.CanDeputyBy(username).Item1;
            });
            Field<DecimalGraphType>("workHour",
                resolve: ctx =>
                {
                    if (ctx.Source.End.HasValue && ctx.Source.Start.HasValue)
                    {
                        return (ctx.UserContext as HrmContext).Holidays.GetWorkHours(ctx.Source.Start.Value, ctx.Source.End.Value);
                    }
                    else
                    {
                        return 0m;
                    }
                });
            Field<BooleanGraphType>("canBalance", resolve: ctx => ctx.Source.CanBalance());
            Field<ListGraphType<StringGraphType>>(
               name: "deputies",
               resolve: context => new List<string>());
            Field<ListGraphType<StringGraphType>>(
               name: "approves",
               resolve: context => new List<string>());

            AddNavigationListField(
                name: "comments",
                resolve: ctx => ctx.Source.Comments);
        }
    }
}
