using BusinessContracts;
using BusinessImplementation;
using DatabaseModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
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
            #region
            services.AddCors();
            services.AddControllers();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IStudentService, StudentService>();

            services.AddDbContext<LibraryManagementContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddEntityFrameworkStores<LibraryManagementContext>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = new TimeSpan(0, 1, 0);
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("admin"));
                options.AddPolicy("StudentOnly", policy => policy.RequireClaim("student"));
            });

            //services.AddScoped<ILoginService, LoginService>();

            #endregion

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1",
                    Description = "API for Library Management",
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            /*app.UseCors(options => options
                 .WithOrigins(
                    new string[]
                    {
                        "http://localhost:3000","http://localhost:4200","http://localhost:8000", "http://localhost:3500", "http://172.16.16.253:8080"
                    }
                 )
                 .AllowAnyHeader().AllowAnyMethod() //.AllowAnyOrigin()
                );*/
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            //app.UseCors(options => options.AllowAnyOrigin().AllowCredentials().AllowAnyHeader());

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
