using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using App2.Models;
using Newtonsoft.Json;

[Authorize]
public class DashboardController : Controller
{
    private readonly IConfiguration _configuration;
    public DashboardController(IConfiguration configuration)
    {
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
    public async Task<IActionResult> Inventory()
    {
        var redirectUrl = $"{_configuration["IdentityServerApplicationUrl"]}/api/inventory";

        using (var handler = new HttpClientHandler())
        {
            if (Request.Cookies.Any())
            {
                handler.CookieContainer = new CookieContainer();
                foreach (var cookie in Request.Cookies)
                {
                    handler.CookieContainer.Add(new Uri(redirectUrl), new Cookie(cookie.Key, cookie.Value));
                }
            }

            using (var client = new HttpClient(handler))
            {
                var response = await client.GetAsync(redirectUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var Inventory = JsonConvert.DeserializeObject<List<Inventory>>(content);
                    return View(Inventory);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Error fetching inventory data");
                }
            }
        }
    }

    [Authorize(Policy = "RequireAdminOrShopperRole")]
    public async Task<IActionResult> Orders()
    {
        var redirectUrl = $"{_configuration["IdentityServerApplicationUrl"]}/api/order";

        using (var handler = new HttpClientHandler())
        {
            if (Request.Cookies.Any())
            {
                handler.CookieContainer = new CookieContainer();
                foreach (var cookie in Request.Cookies)
                {
                    handler.CookieContainer.Add(new Uri(redirectUrl), new Cookie(cookie.Key, cookie.Value));
                }
            }

            using (var client = new HttpClient(handler))
            {
                var response = await client.GetAsync(redirectUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<List<Order>>(content);
                    return View(orders);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Error fetching orders data");
                }
            }
        }
    }

    [Authorize(Policy = "RequireAdminOrShopperRole")]
    public async Task<IActionResult> OrdersByUserId()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID is missing.");
        }

        var redirectUrl = $"{_configuration["IdentityServerApplicationUrl"]}/api/order/{userId}";

        using (var handler = new HttpClientHandler())
        {
            if (Request.Cookies.Any())
            {
                handler.CookieContainer = new CookieContainer();
                foreach (var cookie in Request.Cookies)
                {
                    handler.CookieContainer.Add(new Uri(redirectUrl), new Cookie(cookie.Key, cookie.Value));
                }
            }

            using (var client = new HttpClient(handler))
            {
                var response = await client.GetAsync(redirectUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var OrdersByUserId = JsonConvert.DeserializeObject<List<Order>>(content);
                    return View(OrdersByUserId);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Error fetching orders data");
                }
            }
        }
    }
}