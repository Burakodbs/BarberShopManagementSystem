using BarberShopManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BarberShopManagementSystem.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {

        public DbSet<Salon> Salons { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Customer> Customers { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>().HasData(
                new Service
                {
                    Id = 1,
                    Name = "Saç Kesimi",
                    Price = 250,
                    Duration = 60
                },
                new Service
                {
                    Id = 2,
                    Name = "Sakal Kesimi",
                    Price = 50,
                    Duration = 30
                },
                new Service
                {
                    Id = 3,
                    Name = "Saç Boyama",
                    Price = 400,
                    Duration = 120
                },
                new Service
                {
                    Id = 4,
                    Name = "Cilt Bakımı",
                    Price = 150,
                    Duration = 60
                },
                new Service
                {
                    Id = 5,
                    Name = "Saç Sakal Kesimi",
                    Price = 300,
                    Duration = 90
                }
            );

            base.OnModelCreating(modelBuilder);

            // Salon-Personel İlişkisi
            modelBuilder.Entity<Salon>()
                .HasMany(s => s.Employees)
                .WithOne(e => e.Salon)
                .HasForeignKey(e => e.SalonId)
                .OnDelete(DeleteBehavior.Restrict);

            // Salon-Hizmet İlişkisi
            //modelBuilder.Entity<Salon>()
            //    .HasMany(s => s.Services)
            //    .WithOne(se => se.Salon)
            //    .HasForeignKey(se => se.SalonId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // Randevu İlişkileri
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Salon)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.SalonId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Appointments)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            
        }

    }

}

