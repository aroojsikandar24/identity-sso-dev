using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityServer.Controllers
{
    //[Authorize(Policy = "RequireAdminOrShopperRole")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = _context.Orders.ToList();
            return Ok(orders);
        }

        [HttpGet]
        [Route("{userId}")]
        public IActionResult GetOrdersByUserId(string userId)
        {
            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .ToList(); 

            return Ok(orders);
        }

    }
}
