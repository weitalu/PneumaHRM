using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class LeaveBalance : Entity
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public DateTime ValidThru { get; set; }
        public string SnapShotData { get; set; }

        public string OwnerId { get; set; }
        public Employee Owner { get; set; }

        public List<RequestBalanceRelation> RequestRelations { get; set; } = new List<RequestBalanceRelation>();
    }
    public class LeaveBalanceInputType : InputObjectGraphType<LeaveBalance>
    {
        public LeaveBalanceInputType()
        {
            Name = "LeaveBalanceInput";
            Description = "A leave balance";

            Field<NonNullGraphType<DecimalGraphType>>()
                .Name("value")
                .Description("balance value");
            Field<NonNullGraphType<StringGraphType>>()
                .Name("description")
                .Description("description of balance");
            Field<NonNullGraphType<StringGraphType>>()
                .Name("ownerId")
                .Description("balance to who");
        }
    }
    public class LeaveBalanceType : ObjectGraphType<LeaveBalance>
    {
        public LeaveBalanceType()
        {
            Field<IdGraphType>("id", resolve: ctx => ctx.Source.Id);
            Field(x => x.Description);
            Field(x => x.Value);
            Field(x => x.SnapShotData, true);
            Field<DateTimeGraphType>("createdOn", resolve: ctx => ctx.Source.CreatedOn);
            Field<StringGraphType>("createdBy", resolve: ctx => ctx.Source.CreatedBy);
            Field<StringGraphType>("ownerId", resolve: ctx => ctx.Source.OwnerId);
        }
    }
}
