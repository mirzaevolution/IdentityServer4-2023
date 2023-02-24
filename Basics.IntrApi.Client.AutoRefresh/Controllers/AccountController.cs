using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basics.IntrApi.Client.AutoRefresh.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login(string redirectUri = "/")
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Url.IsLocalUrl(redirectUri) ? redirectUri : Url.Action("Index", "Home")
            }, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [Authorize]
        public IActionResult Logout()
        {
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
