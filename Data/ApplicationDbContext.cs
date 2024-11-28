using BarberShopManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberShopManagementSystem.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        // Veri tabanı tabloları burada tanımlanacak.
        public DbSet<Salon> Salons { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Customer> Customers { get; set; }


    }
}
