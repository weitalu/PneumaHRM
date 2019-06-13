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
        Comment, Approve, CancelApprove, Deputy, CancelDeputy
    }
    public class LeaveRequestCommentInputType : InputObjectGraphType<LeaveRequestComment>
    {
        public LeaveRequestCommentInputType()
        {
            Name = "LeaveRequestCommentInput";
            Description = "A leave request comment";
            //https://github.com/graphql-dotnet/graphql-dotnet/issues/1017 
            //Resolve(ctx => ctx.Source.Content); not support, the field name must the same with property name
            Field<StringGraphType>()
                .Name("content")
                .Description("content of the comment");
            Field<NonNullGraphType<IntGraphType>>()
                .Name("requestId")
                .Description("the target to comment");
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
