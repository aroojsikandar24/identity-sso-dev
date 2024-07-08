using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace App2.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Silent()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = _configuration["ApplicationUrl"] }, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
