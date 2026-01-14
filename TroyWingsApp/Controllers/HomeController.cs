using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TroyWingsApp.Models;
using TroyWingsApp.Services;

namespace TroyWingsApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRegistrationService _registrationService;

    public HomeController(ILogger<HomeController> logger, IRegistrationService registrationService)
    {
        _logger = logger;
        _registrationService = registrationService;
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

        _registrationService.Register(registration);

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
