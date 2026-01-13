using MySqlConnector;
using TroyWingsApp.Models;

namespace TroyWingsApp.Data;

public interface IRegistrationRepository
{
    void EnsureDatabaseSetup();
    void Save(Registration registration);
}

public class MySqlRegistrationRepository : IRegistrationRepository
{
    private readonly string _connectionString;
    private readonly ILogger<MySqlRegistrationRepository> _logger;

    public MySqlRegistrationRepository(IConfiguration configuration, ILogger<MySqlRegistrationRepository> logger)
    {
        _connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not found.");
        _logger = logger;
    }

    public void EnsureDatabaseSetup()
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
    }

    public void Save(Registration registration)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        const string insertSql = """
            INSERT INTO Registrations
                (Name, FatherName, DateOfBirth, ContactNumber, Address, CreatedAtUtc)
            VALUES
                (@Name, @FatherName, @DateOfBirth, @ContactNumber, @Address, @CreatedAtUtc);
            """;

        using var command = new MySqlCommand(insertSql, connection);
        command.Parameters.AddWithValue("@Name", registration.Name);
        command.Parameters.AddWithValue("@FatherName", registration.FatherName);
        command.Parameters.AddWithValue("@DateOfBirth", registration.DateOfBirth?.ToDateTime(TimeOnly.MinValue));
        command.Parameters.AddWithValue("@ContactNumber", registration.ContactNumber);
        command.Parameters.AddWithValue("@Address", registration.Address);
        command.Parameters.AddWithValue("@CreatedAtUtc", registration.CreatedAtUtc);

        var affected = command.ExecuteNonQuery();
        if (affected != 1)
        {
            _logger.LogWarning("Unexpected rows affected when inserting registration: {Count}", affected);
        }
    }
}
