using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphiQl;
using GraphQL;
using GraphQL.EntityFramework;
using GraphQL.Types;
using GraphQL.Types.Relay;
using GraphQL.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PneumaHRM.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Http.Headers;

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
            GraphTypeTypeRegistry.Register<Holiday, HolidayType>();
            GraphTypeTypeRegistry.Register<LeaveRequest, LeaveRequestType>();
            GraphTypeTypeRegistry.Register<LeaveBalance, LeaveBalanceType>();
            GraphTypeTypeRegistry.Register<LeaveRequestComment, LeaveRequestCommentType>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "GitHub";
            })
                .AddCookie()
                .AddOAuth("GitHub", options =>
                {
                    options.ClientId = Configuration["GitHub:ClientId"];
                    options.ClientSecret = Configuration["GitHub:ClientSecret"];
                    options.CallbackPath = new PathString("/signin-github");

                    options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                    options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                    options.UserInformationEndpoint = "https://api.github.com/user";

                    options.SaveTokens = true;

                    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                    options.ClaimActions.MapJsonKey("urn:github:login", "login");
                    options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
                    options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");

                    options.Events = new OAuthEvents
                    {
                        OnCreatingTicket = async context =>
                        {
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();

                            var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                            context.RunClaimActions(user);
                        }
                    };
                });
            services.AddHttpContextAccessor();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddEntityFrameworkSqlite()
                .AddDbContext<HrmDbContext>();
            foreach (var type in GetGraphQlTypes())
            {
                services.AddSingleton(type);
            }

            EfGraphQLConventions.RegisterConnectionTypesInContainer(services);
            EfGraphQLConventions.RegisterInContainer(services, HrmDbContext.DataModel);
            services.AddSingleton<IDocumentExecuter, EfDocumentExecuter>();
            services.AddSingleton<ISchema>(provider => new HrmSchema(new FuncDependencyResolver(provider.GetRequiredService)));
            services.AddSingleton(x => new List<DateTime>());
            services.Configure<GzipCompressionProviderOptions>(opt => opt.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, HrmDbContext db, List<DateTime> Holidays)
        {
            app.UseResponseCompression();
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
            //app.UseHttpsRedirection();
            app.UseGraphiQl("/GraphiQL", "/api/graphql");

            app.UseMvc();
            app.UseStaticFiles();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "wwwroot";
            });
            db.SeedData();
            Holidays.AddRange(db.Holidays.Select(x => x.Value).ToList());
        }


        static IEnumerable<Type> GetGraphQlTypes()
        {
            return typeof(Startup).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract &&
                            (typeof(IGraphType).IsAssignableFrom(x)));
        }
    }
}
