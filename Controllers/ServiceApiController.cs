using BarberShopManagementSystem.Data;
using BarberShopManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShopManagementSystem.Controllers.Api {
    [Route("api/services")]
    [ApiController]
    public class ServicesApiController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public ServicesApiController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices() {
            var services = await _context.Services
                .Select(s => new Service {
                    Id = s.Id,
                    Name = s.Name,
                    Price = s.Price,
                    Duration = s.Duration
                })
                .OrderBy(s => s.Price)
                .ToListAsync();

            return Ok(services);
        }
    }
}