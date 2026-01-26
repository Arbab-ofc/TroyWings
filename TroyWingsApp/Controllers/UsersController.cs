using Microsoft.AspNetCore.Mvc;
using TroyWingsApp.Models;
using TroyWingsApp.Services;

namespace TroyWingsApp.Controllers;

public class UsersController : Controller
{
    private readonly IRegistrationService _registrationService;

    public UsersController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    public IActionResult Index()
    {
        var users = _registrationService.GetAllRegistrations();
        return View(users);
    }

    [HttpGet]
    public IActionResult List()
    {
        try
        {
            var users = _registrationService.GetAllRegistrations();
            return Json(new { items = users });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = ex.Message,
                detail = ex.GetType().Name
            });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update([FromBody] UpdateRegistrationRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { message = "Request payload is missing." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Please correct the highlighted fields." });
        }

        if (request.Id <= 0)
        {
            return BadRequest(new { message = "User id is missing." });
        }

        if (!DateOnly.TryParse(request.DateOfBirth, out var dateOfBirth))
        {
            return BadRequest(new { message = "Date of birth must be a valid date." });
        }

        var updated = _registrationService.UpdateRegistration(new Registration
        {
            Id = request.Id,
            Name = request.Name.Trim(),
            FatherName = request.FatherName.Trim(),
            DateOfBirth = dateOfBirth,
            ContactNumber = request.ContactNumber.Trim(),
            Address = request.Address.Trim()
        });

        if (!updated)
        {
            return BadRequest(new { message = "Unable to update the selected user." });
        }

        return Ok(new { success = true });
    }
}
