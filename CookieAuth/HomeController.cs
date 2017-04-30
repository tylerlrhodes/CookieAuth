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
      return "<a href=\"/home/test\">Test</a> <br /> <a href=\"/home/test2\">Test 2</a>";
    }

    [HttpGet]
    public async Task<IActionResult> Login(string username, string password)
    {
      _logger.LogInformation($"{username}, {password}");

      if (password == "dumb")
      {
        var claims = new List<Claim>
        {
          new Claim("UserName", "Tyler"),
          new Claim(ClaimTypes.Name, "TylerRhodes")
        };

        var identity = new ClaimsIdentity(claims, "TestInstance");

        await HttpContext.Authentication.SignInAsync("TestInstance", new ClaimsPrincipal(identity), new AuthenticationProperties());
      }
      return RedirectToAction("Index");
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
