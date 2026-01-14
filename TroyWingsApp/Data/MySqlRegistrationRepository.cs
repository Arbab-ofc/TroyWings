using System.Data;
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
    private const string CreateRegistrationProcedure = "sp_create_registration";
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

        
        using var command = new MySqlCommand("""
            SELECT ROUTINE_NAME
            FROM INFORMATION_SCHEMA.ROUTINES
            WHERE ROUTINE_SCHEMA = DATABASE()
              AND ROUTINE_NAME = @procedure
              AND ROUTINE_TYPE = 'PROCEDURE'
            LIMIT 1;
            """, connection);
        command.Parameters.AddWithValue("@procedure", CreateRegistrationProcedure);

        var exists = command.ExecuteScalar();
        if (exists is null)
        {
            _logger.LogWarning("Stored procedure {Procedure} was not found in the current schema.", CreateRegistrationProcedure);
        }
    }

    public void Save(Registration registration)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var command = new MySqlCommand(CreateRegistrationProcedure, connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@p_name", registration.Name);
        command.Parameters.AddWithValue("@p_father_name", registration.FatherName);
        command.Parameters.AddWithValue("@p_date_of_birth", registration.DateOfBirth?.ToDateTime(TimeOnly.MinValue));
        command.Parameters.AddWithValue("@p_contact_number", registration.ContactNumber);
        command.Parameters.AddWithValue("@p_address", registration.Address);
        command.Parameters.AddWithValue("@p_created_at_utc", registration.CreatedAtUtc);

        var affected = command.ExecuteNonQuery();
        if (affected != 1)
        {
            _logger.LogWarning("Unexpected rows affected when inserting registration: {Count}", affected);
        }
    }
}
