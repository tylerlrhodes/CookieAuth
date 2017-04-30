using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CookieAuth
{

  public class HomeController : Controller
  {
    private ILogger _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    [Produces("text/html")]
    public string Index()
    {
      return "<a href=\"/home/test\">Test</a> <br /> " +
             "<a href=\"/home/test2\">Test 2</a> <br />" +
             "<a href=\"/home/LoginMessage\">Login Link</a><br />" +
             "<a href=\"/home/Logout\">Log out</a><br />" +
             "<a href=\"/home/RoleAuth1\">Role</a><br />" +
             "<a href=\"/home/RoleAuth2\">Role 2</a><br />";
    }

    [Produces("text/html")]
    public string LoginMessage()
    {
      return "You need to login, this link will log you in: <a href=\"/home/login?password=dumb\">log me in</a>"; 
    }

    [HttpGet]
    public async Task<IActionResult> Login(string username, string password)
    {
      _logger.LogInformation($"Attempted sign in - {username}, {password}");
      // This is where you would check the username and password against the database
      // Then you would sign the user in such as below is doing
      // This creates a cookie sent down to the browser with the Claims
      // We just check that the password is equal to "dumb"
      if (password == "dumb")
      {
        var claims = new List<Claim>
        {
          new Claim("UserName", "Tyler"),
          new Claim(ClaimTypes.Name, "TylerRhodes"),
          new Claim(ClaimTypes.Role, "RoleAuth1")
        };

        var identity = new ClaimsIdentity(claims, "TestInstance");

        await HttpContext.Authentication.SignInAsync("TestInstance", new ClaimsPrincipal(identity),
          new AuthenticationProperties()
          {
            IsPersistent = true
          });
      }
      return RedirectToAction("Index");
    }

    public async Task<string> Logout()
    {
      if (HttpContext.User.Identity.IsAuthenticated)
      {
        await HttpContext.Authentication.SignOutAsync("TestInstance");
        return "Signed out the user!";
      }
      else
      {
        return "User not logged in!";
      }
    }

    [Authorize(Roles ="RoleAuth1")]
    public string RoleAuth1()
    {
      return "You got to RoleAuth1";
    }

    [Authorize(Roles ="RoleAuth2")]
    public string RoleAuth2()
    {
      return "If you see this, something is wrong!";
    }

    [Authorize(Policy = "YouMayPass")]
    public string Test()
    {
      return $"Worked {HttpContext.User.Identity.Name}";
    }

    [Authorize(Policy = "YouMayNotPass")]
    public string Test2()
    {
      return "Error if you see this!";
    }

    public string FailedAccess()
    {
      return "Error you are not authorized!!!";
    }
  }
}
