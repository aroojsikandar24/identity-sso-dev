using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Secure()
    {
        return View();
    }

    [Authorize]
    public IActionResult GetUserInfo()
    {
        var userInfo = new
        {
            User.Identity.Name,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        };
        return Json(userInfo);
    }

    public async Task<IActionResult> Logout()
    {
        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5000");
        if (disco.IsError)
        {
            throw new System.Exception($"Error getting discovery document: {disco.Error}");
        }

        var idToken = await HttpContext.GetTokenAsync("id_token");

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

        var logoutUrl = $"{disco.EndSessionEndpoint}?id_token_hint={idToken}&post_logout_redirect_uri=https://localhost:5002";

        return Redirect(logoutUrl);
    }

    public IActionResult Login()
    {
        var returnUrl = "https://localhost:5002";
        var redirectUrl = Url.Action("Login", "Account", new { returnUrl });
        return Challenge(new AuthenticationProperties { RedirectUri = "https://localhost:5002" }, OpenIdConnectDefaults.AuthenticationScheme);
    }

    public IActionResult Register()
    {
        var returnUrl = "https://localhost:5002";
        var redirectUrl = $"https://localhost:5000/Account/Register?returnUrl={returnUrl}";
        return Redirect(redirectUrl);
    }
}

public class LogoutRequest
{
    public string IdTokenHint { get; set; }
    public string PostLogoutRedirectUri { get; set; }

    public string ToQueryString()
    {
        return $"id_token_hint={Uri.EscapeDataString(IdTokenHint)}&post_logout_redirect_uri={Uri.EscapeDataString(PostLogoutRedirectUri)}";
    }
}