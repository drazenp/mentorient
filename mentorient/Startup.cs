﻿using mentorient.Data;
using mentorient.Models;
using mentorient.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace mentorient
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
         services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

         services.AddIdentity<ApplicationUser, IdentityRole>(config =>
             {
                config.SignIn.RequireConfirmedEmail = true;
             })
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

         services.AddAuthentication()
           .AddCookie(cfg => cfg.SlidingExpiration = true)
           .AddJwtBearer(cfg =>
           {
              cfg.RequireHttpsMetadata = false;
              cfg.SaveToken = true;

              cfg.TokenValidationParameters = new TokenValidationParameters()
              {
                 ValidIssuer = Configuration["Tokens:Issuer"],
                 ValidAudience = Configuration["Tokens:Issuer"],
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
              };
           });

         // Add application services.
         services.AddTransient<IEmailSender, EmailSender>();

         services.AddMvc();

         services.Configure<AuthMessageSenderOptions>(Configuration);
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
            app.UseBrowserLink();
            app.UseDatabaseErrorPage();
         }
         else
         {
            app.UseExceptionHandler("/Home/Error");
         }

         app.UseStaticFiles();

         app.UseAuthentication();

         app.UseMvc(routes =>
         {
            routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");
         });
      }
   }
}
