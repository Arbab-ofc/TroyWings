using TroyWingsApp.Data;
using TroyWingsApp.Models;

namespace TroyWingsApp.Services;

public interface IRegistrationService
{
    void Register(Registration registration);
    PagedResult<Registration> GetRegistrations(int page, int pageSize);
    IReadOnlyList<Registration> GetAllRegistrations();
    bool UpdateRegistration(Registration registration);
}

public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository _repository;
    private readonly ILogger<RegistrationService> _logger;

    public RegistrationService(IRegistrationRepository repository, ILogger<RegistrationService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public void Register(Registration registration)
    {
        if (registration == null)
        {
            throw new ArgumentNullException(nameof(registration));
        }

        registration.CreatedAtUtc = DateTime.UtcNow;

        try
        {
            _repository.Save(registration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to register user {Name}", registration.Name);
            throw;
        }
    }

    public PagedResult<Registration> GetRegistrations(int page, int pageSize)
    {
        return _repository.GetPage(page, pageSize);
    }

    public IReadOnlyList<Registration> GetAllRegistrations()
    {
        return _repository.GetAll();
    }

    public bool UpdateRegistration(Registration registration)
    {
        if (registration == null)
        {
            throw new ArgumentNullException(nameof(registration));
        }

        try
        {
            return _repository.Update(registration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update registration {Id}", registration.Id);
            throw;
        }
    }
}
