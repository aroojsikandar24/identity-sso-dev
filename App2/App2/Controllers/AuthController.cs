using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace App2.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Silent()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "https://localhost:5002" }, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
