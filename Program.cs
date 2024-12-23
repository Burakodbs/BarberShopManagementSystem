using Microsoft.EntityFrameworkCore;
using BarberShopManagementSystem.Data;
using Microsoft.AspNetCore.Identity;
using BarberShopManagementSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity with more flexible password options
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Relax password requirements for testing
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// Seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    try
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // Create Admin role if it doesn't exist
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!roleResult.Succeeded)
            {
                // Log role creation errors
                foreach (var error in roleResult.Errors)
                {
                    Console.WriteLine($"Role Creation Error: {error.Description}");
                }
            }
        }

        // Create default admin user
        var adminEmail = "b221210078@sakarya.edu.tr";
        var adminPassword = "sau123"; 

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {

                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(adminUser, adminPassword);
            if (createResult.Succeeded)
            {
                var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                if (!roleResult.Succeeded)
                {
                    // Log role assignment errors
                    foreach (var error in roleResult.Errors)
                    {
                        Console.WriteLine($"Role Assignment Error: {error.Description}");
                    }
                }
            }
            else
            {
                // Log user creation errors
                foreach (var error in createResult.Errors)
                {
                    Console.WriteLine($"User Creation Error: {error.Description}");
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Log any unexpected errors
        Console.WriteLine($"Unexpected Error: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
