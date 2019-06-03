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

            Field<LeaveBalanceType, LeaveBalance>()
                .Name("createLeaveBalance")
                .Argument<NonNullGraphType<LeaveBalanceInputType>>("input", "The balance you want to create")
                .BuildMutationResolver<LeaveBalance, LeaveBalance>(HRMUtility.CreateLeaveBalance);

            Field<LeaveRequestType, LeaveRequest>()
                .Name("approveLeaveRequest")
                .Argument<NonNullGraphType<LeaveRequestCommentInputType>>("input", "the form data to approve")
                .BuildMutationResolver<LeaveRequest, LeaveRequestComment>(HRMUtility.ApproveLeaveRequest);

            Field<LeaveRequestType, LeaveRequest>()
                .Name("deputyLeaveRequest")
                .Argument<NonNullGraphType<LeaveRequestCommentInputType>>("input", "the form data to approve")
                .BuildMutationResolver<LeaveRequest, LeaveRequestComment>(HRMUtility.DeputyLeaveRequest);

            Field<LeaveRequestType, LeaveRequest>()
                 .Name("completeLeaveRequest")
                 .Argument<NonNullGraphType<IdGraphType>>("input", "The Id of the target leave request to be complete")
                 .BuildMutationResolver<LeaveRequest, int>(HRMUtility.CompleteLeaveRequest);

            Field<LeaveRequestType, LeaveRequest>()
                 .Name("commentLeaveRequest")
                 .Argument<NonNullGraphType<LeaveRequestCommentInputType>>("input", "The Id of the target leave request to be complete")
                 .BuildMutationResolver<LeaveRequest, LeaveRequestComment>(HRMUtility.CommentLeaveRequest);
        }
    }
}
