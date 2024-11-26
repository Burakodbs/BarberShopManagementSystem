using BarberShopManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberShopManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Veri tabanı tabloları burada tanımlanacak.
        public DbSet<Salon> Salons { get; set; }
        public DbSet<Employee> Employees { get; set; }

    }
}
