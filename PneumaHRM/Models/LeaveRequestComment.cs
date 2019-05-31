using GraphQL.EntityFramework;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PneumaHRM.Models
{
    public class LeaveRequestComment : Entity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public CommentType Type { get; set; }
        public int? RequestId { get; set; }
        public LeaveRequest Request { get; set; }

    }

    public enum CommentType
    {
        None, Approve, CancelApprove, Deputy, CancelDeputy
    }
    public class LeaveRequestCommentInputType : InputObjectGraphType<LeaveRequestComment>
    {
        public LeaveRequestCommentInputType()
        {
            Name = "LeaveRequestCommentInput";
            Description = "A leave request comment";

            Field<StringGraphType>()
                .Name("content")
                .Description("content of the comment")
                .Resolve(ctx => ctx.Source.Content);
            Field<NonNullGraphType<IntGraphType>>()
                .Name("leaveRequestId")
                .Description("the target to comment")
                .Resolve(ctx => ctx.Source.RequestId);
        }
    }
    public class LeaveRequestCommentType : EfObjectGraphType<LeaveRequestComment>
    {
        public LeaveRequestCommentType(IEfGraphQLService service) : base(service)
        {
            Field<DateTimeGraphType>("createdOn", resolve: ctx => ctx.Source.CreatedOn);
            Field<StringGraphType>("createdBy", resolve: ctx => ctx.Source.CreatedBy);
            Field<StringGraphType>("content", resolve: ctx => ctx.Source.Content);
            Field<CommentTypeEnum>("type", resolve: ctx => ctx.Source.Type);
        }
    }
    public class CommentTypeEnum : EnumerationGraphType<CommentType>
    {
    }
}
