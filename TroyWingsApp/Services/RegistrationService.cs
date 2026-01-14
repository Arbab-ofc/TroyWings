using TroyWingsApp.Data;
using TroyWingsApp.Models;

namespace TroyWingsApp.Services;

public interface IRegistrationService
{
    void Register(Registration registration);
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
}
