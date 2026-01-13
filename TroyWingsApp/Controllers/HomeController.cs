using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TroyWingsApp.Data;
using TroyWingsApp.Models;

namespace TroyWingsApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRegistrationRepository _repository;

    public HomeController(ILogger<HomeController> logger, IRegistrationRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public IActionResult Index()
    {
        return View(new Registration());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(Registration registration)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please fix the highlighted fields to continue.";
            return View("Index", registration);
        }

        registration.CreatedAtUtc = DateTime.UtcNow;
        _repository.Save(registration);

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
