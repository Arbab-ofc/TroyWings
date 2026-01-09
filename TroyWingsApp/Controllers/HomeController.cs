using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TroyWingsApp.Data;
using TroyWingsApp.Models;

namespace TroyWingsApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        return View(new Registration());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(Registration registration)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please fix the highlighted fields to continue.";
            return View("Index", registration);
        }

        registration.CreatedAtUtc = DateTime.UtcNow;
        _db.Registrations.Add(registration);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Registration submitted successfully.";
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
