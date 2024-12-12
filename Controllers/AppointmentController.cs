using BarberShopManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using BarberShopManagementSystem.Data;
using BarberShopManagementSystem.Controllers;
using Microsoft.EntityFrameworkCore;
using BarberShopManagementSystem.ViewModels;

public class AppointmentController : Controller
{
    private readonly ApplicationDbContext _context;

    public AppointmentController(ApplicationDbContext context)
    {
        _context = context;
    }

}



