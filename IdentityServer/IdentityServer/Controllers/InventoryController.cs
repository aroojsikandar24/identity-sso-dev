using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Controllers
{
    //[Authorize(Policy = "RequireAdminRole")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetInventory()
        {
            var inventory = _context.Inventory.ToList(); 

            return Ok(inventory);
        }

    }
}
