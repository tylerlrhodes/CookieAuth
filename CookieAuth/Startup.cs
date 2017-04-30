using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IO;
using Microsoft.AspNetCore.DataProtection;

namespace CookieAuth
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();

      services.AddAuthorization(options =>
      {
        options.AddPolicy("YouMayPass", policy => policy.RequireClaim("UserName"));
        options.AddPolicy("YouMayNotPass", policy => policy.RequireClaim("Password"));
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseCookieAuthentication(new CookieAuthenticationOptions()
      {
        AuthenticationScheme = "TestInstance",
        LoginPath = new PathString("/Home/LoginMessage"),
        AccessDeniedPath = new PathString("/Home/FailedAccess"),
        AutomaticAuthenticate = true,
        AutomaticChallenge = true,
        CookieName = "CookieAuthDemo",
        ExpireTimeSpan = TimeSpan.FromMinutes(10)
      });

      app.UseMvcWithDefaultRoute();

    }
  }
}

