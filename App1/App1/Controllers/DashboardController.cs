using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;

[Authorize]
public class DashboardController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public DashboardController(IHttpClientFactory httpClientFactory, IConfiguration configuration, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Secure()
    {
        return View();
    }

    public IActionResult UserInfo()
    {
        var userInfo = new
        {
            User.Identity.Name,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        };
        return View(userInfo);
    }

    [Authorize(Policy = "RequireAdminRole")]
    public async Task<IActionResult> GetInventory()
    {

        var redirectUrl = " https://localhost:5000/api/inventory";
        return Redirect(redirectUrl);

        /*  var client = _httpClientFactory.CreateClient();
          var accessToken = await HttpContext.GetTokenAsync("access_token");
          if (string.IsNullOrEmpty(accessToken))
          {
              return Unauthorized("Access token is missing.");
          }

          client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

          var response = await client.GetAsync("https://localhost:5000/api/inventory");

          if (response.IsSuccessStatusCode)
          {
              var inventoryData = await response.Content.ReadAsStringAsync();
              return Content(inventoryData, "application/json");
          }
          else
          {
              var errorContent = await response.Content.ReadAsStringAsync();
              return StatusCode((int)response.StatusCode, errorContent);
          }*/
    }

    [Authorize(Policy = "RequireAdminOrShopperRole")]
    public async Task<IActionResult> GetOrders()
    {
        var redirectUrl = " https://localhost:5000/api/order";
        return Redirect(redirectUrl);


        /*    var accessToken = await HttpContext.GetTokenAsync("access_token");

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accessToken) as JwtSecurityToken;

            var user = HttpContext.User;
            var scopes = user.FindAll("scope").Select(c => c.Value);
            var audience = user.FindFirst("aud")?.Value;

            if (string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized("Access token is missing.");
            }


            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.GetAsync("https://localhost:5000/api/order");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseData);
                    return Content(responseData, "application/json");
                }
            }
            return StatusCode((int)404, "");*/

    }

/*    [Authorize(Policy = "RequireAdminOrShopperRole")]
    public async Task<IActionResult> GetOrdersByUserId()
    {
        var userId = User.Identity.id;
        var redirectUrl = $"http://localhost:5000/api/order/{userId}";
        return Redirect(redirectUrl);
    }*/
}