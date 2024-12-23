using Microsoft.AspNetCore.Mvc;
using BarberShopManagementSystem.Data;
using BarberShopManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberShopManagementSystem.Controllers.Api
{
    [Route("api/services")]
    [ApiController]
    public class ServicesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServicesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            var services = await _context.Services.ToListAsync();
            return Ok(services);
        }
    }
}