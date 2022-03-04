using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentitySample.Server.Controllers;

/// <summary>
/// 
/// </summary>
[AllowAnonymous]
public class AccountController : Controller
{
    [HttpPost]
    public async Task<IActionResult> Login(string? userName, string? password)
    {
        // check username and password
        var auth = true;
        return auth ? await SignInAsync(userName ?? "admin") : Redirect("/Account/Login");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

    private async Task<IActionResult> SignInAsync(string userName, string authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme)
    {
        var identity = new ClaimsIdentity(authenticationScheme);
        identity.AddClaim(new Claim(ClaimTypes.Name, userName));
        identity.AddClaim(new Claim(ClaimTypes.Email, "email@email.com"));
        identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
        identity.AddClaim(new Claim(ClaimTypes.Role, "CanEditUsers"));


        var properties = new AuthenticationProperties();
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

        return Redirect("/");
    }
}
