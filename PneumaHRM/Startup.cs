using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphiQl;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PneumaHRM.Models;

namespace PneumaHRM
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<HrmDbContext>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<ISchema>(new HrmSchema());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, HrmDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.Use(async (context, next) =>
            {
                var Id = context.User.Identity;
                if (Id.IsAuthenticated)
                {
                    var dbCtx = context.RequestServices.GetService<HrmDbContext>();
                    var user = dbCtx.Employees.Where(x => x.ADPrincipalName == Id.Name).FirstOrDefault();
                    if (user == null)
                    {
                        dbCtx.Employees.Add(new Employee()
                        {
                            ADPrincipalName = Id.Name,
                            isActive = true
                        });
                        dbCtx.SaveChanges();
                    }
                }
                // Do work that doesn't write to the Response.
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
            });
            app.UseHttpsRedirection();
            app.UseGraphiQl("/GraphiQL", "/api/graphql");

            app.UseMvc();
            app.UseStaticFiles();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "wwwroot";
            });
            db.SeedData();
            ;
        }
    }
}
