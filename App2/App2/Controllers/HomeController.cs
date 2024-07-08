using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public HomeController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Secure()
    {
        return View();
    }

    [Authorize]
    public IActionResult Login()
    {
        var properties = new AuthenticationProperties { RedirectUri = $"{_configuration["ApplicationUrl"]}/Dashboard" };
        return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
    }

    public IActionResult Register()
    {
        var returnUrl = _configuration["ApplicationUrl"];
        var redirectUrl = $"{_configuration["IdentityServerApplicationUrl"]}/Account/Register?returnUrl={returnUrl}";
        return Redirect(redirectUrl);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync(_configuration["IdentityServerApplicationUrl"]);
        if (disco.IsError)
        {
            throw new System.Exception($"Error getting discovery document: {disco.Error}");
        }

        var idToken = await HttpContext.GetTokenAsync("id_token");

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

        var logoutUrl = $"{disco.EndSessionEndpoint}?id_token_hint={idToken}&post_logout_redirect_uri={_configuration["ApplicationUrl"]}";

        return Redirect(logoutUrl);
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