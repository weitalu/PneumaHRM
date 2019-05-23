﻿using GraphQL.EntityFramework;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


namespace PneumaHRM.Models
{
    public class HrmQuery : EfObjectGraphType
    {
        public HrmQuery(IEfGraphQLService efGraphQLService) : base(efGraphQLService)
        {
            AddQueryField(
                "holidays",
                resolve: ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var db = hrmCtx.DbContext;
                    return db.Holidays;
                });
            AddQueryField(
                "leaveBalances",
                resolve: ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var db = hrmCtx.DbContext;
                    return db.LeaveBalances.Include("RequestRelations");
                });
            AddQueryConnectionField(
                "leaveBalancesConnection",
                resolve: ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var db = hrmCtx.DbContext;
                    return db.LeaveBalances.Include("RequestRelations");
                });
            AddQueryField(
                "leaveRequests",
                resolve: ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var db = hrmCtx.DbContext;
                    return db.LeaveRequests.Include("Comments");
                });
            AddQueryConnectionField(
                "leaveRequestsConnection",
                resolve: ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var db = hrmCtx.DbContext;
                    return db.LeaveRequests;
                });
            AddQueryField(
              "leaveRequestComments",
              resolve: ctx =>
              {
                  var hrmCtx = ctx.UserContext as HrmContext;
                  var db = hrmCtx.DbContext;
                  return db.LeaveRequestComments;
              });
            Field<ListGraphType<EmployeeType>>(
                "employees",
                resolve: ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var db = hrmCtx.DbContext;
                    return db.Employees.Include("Balances").ToList();
                });
            Field<DecimalGraphType>(
                "workHours",
                 arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<DateTimeGraphType>> { Name = "from" },
                    new QueryArgument<NonNullGraphType<DateTimeGraphType>> { Name = "to" }
                ),
                resolve: ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var from = ctx.GetArgument<DateTime>("from");
                    var to = ctx.GetArgument<DateTime>("to");
                    return hrmCtx.Holidays.GetWorkHours(from, to);
                });
            Field<EmployeeType>(
                "self",
                resolve: ctx =>
                {
                    var hrmCtx = ctx.UserContext as HrmContext;
                    var db = hrmCtx.DbContext;
                    var userName = hrmCtx.UserContext.Identity.Name;
                    return db.Employees.Where(x => x.ADPrincipalName == userName).Include("Balances").FirstOrDefault();
                });
        }
    }
}
