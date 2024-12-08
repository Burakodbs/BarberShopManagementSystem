using BarberShopManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BarberShopManagementSystem.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Salon> Salons { get; set; }
        public DbSet<Service> SalonServices { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeSpecialty> EmployeeSpecialties { get; set; }
        public DbSet<Appointment> Appointments { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            // Seed Data for Salons
            modelBuilder.Entity<Salon>().HasData(
                new Salon { Id = 1, Name = "Elite Barber", StartHour = new TimeSpan(8, 0, 0), EndHour = new TimeSpan(18, 0, 0) }

            );

            // Seed Data for Employees
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "Ahmet Yılmaz", AvailableFrom = new TimeSpan(8, 0, 0), AvailableTo = new TimeSpan(16, 0, 0), SalonId = 1 },
                new Employee { Id = 2, Name = "Mehmet Kaya", AvailableFrom = new TimeSpan(12, 0, 0), AvailableTo = new TimeSpan(20, 0, 0), SalonId = 1 }
            );

            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, SalonId = 1, ServiceName = "Saç Tıraşı", Duration = 60, Price = 250 },
                new Service { Id = 2, SalonId = 1, ServiceName = "Sakal Tıraşı", Duration = 30, Price = 50 },
                new Service { Id = 3, SalonId = 1, ServiceName = "Saç Sakal Tıraşı", Duration = 90, Price = 300 },
                new Service { Id = 4, SalonId = 1, ServiceName = "Damat Tıraşı", Duration = 60, Price = 500 }
            );

            // Seed Data for Appointments
            //modelBuilder.Entity<Appointment>().HasData(
            //    new Appointment
            //    {
            //        Id = 1,
            //        SalonId = 1,
            //        EmployeeId = 1,
            //        ServiceId = 1,
            //        CustomerId = "user1", // Kullanıcı kimliği
            //        StartTime = DateTime.Today.AddHours(9),
            //        EndTime = DateTime.Today.AddHours(9).AddMinutes(30),
            //        IsApproved = true
            //    },
            //    new Appointment
            //    {
            //        Id = 2,
            //        SalonId = 1,
            //        EmployeeId = 2,
            //        ServiceId = 3,
            //        CustomerId = "user2",
            //        StartTime = DateTime.Today.AddHours(14),
            //        EndTime = DateTime.Today.AddHours(14).AddMinutes(90),
            //        IsApproved = false
            //    }
            //);
        }
        public DbSet<BarberShopManagementSystem.Models.Appointment> Appointment { get; set; } = default!;



    }

}

