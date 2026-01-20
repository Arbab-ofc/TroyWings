using System.Data;
using MySqlConnector;
using TroyWingsApp.Models;

namespace TroyWingsApp.Data;

public interface IRegistrationRepository
{
    void EnsureDatabaseSetup();
    void Save(Registration registration);
    PagedResult<Registration> GetPage(int page, int pageSize);
    IReadOnlyList<Registration> GetAll();
    bool Update(Registration registration);
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

    public PagedResult<Registration> GetPage(int page, int pageSize)
    {
        var safePage = Math.Max(1, page);
        var safePageSize = Math.Clamp(pageSize, 1, 24);

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var countCommand = new MySqlCommand("SELECT COUNT(*) FROM Registrations;", connection);
        var totalCount = Convert.ToInt32(countCommand.ExecuteScalar() ?? 0);

        var offset = (safePage - 1) * safePageSize;
        using var listCommand = new MySqlCommand("""
            SELECT Id, Name, FatherName, DateOfBirth, ContactNumber, Address, CreatedAtUtc
            FROM Registrations
            ORDER BY CreatedAtUtc DESC
            LIMIT @limit OFFSET @offset;
            """, connection);
        listCommand.Parameters.AddWithValue("@limit", safePageSize);
        listCommand.Parameters.AddWithValue("@offset", offset);

        using var reader = listCommand.ExecuteReader();
        var items = new List<Registration>();
        while (reader.Read())
        {
            items.Add(new Registration
            {
                Id = reader.GetInt32("Id"),
                Name = reader.GetString("Name"),
                FatherName = reader.GetString("FatherName"),
                DateOfBirth = reader.IsDBNull("DateOfBirth")
                    ? null
                    : DateOnly.FromDateTime(reader.GetDateTime("DateOfBirth")),
                ContactNumber = reader.GetString("ContactNumber"),
                Address = reader.GetString("Address"),
                CreatedAtUtc = reader.GetDateTime("CreatedAtUtc")
            });
        }

        return new PagedResult<Registration>
        {
            Items = items,
            Page = safePage,
            PageSize = safePageSize,
            TotalCount = totalCount
        };
    }

    public IReadOnlyList<Registration> GetAll()
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var command = new MySqlCommand("""
            SELECT Id, Name, FatherName, DateOfBirth, ContactNumber, Address, CreatedAtUtc
            FROM Registrations
            ORDER BY CreatedAtUtc DESC;
            """, connection);

        using var reader = command.ExecuteReader();
        var items = new List<Registration>();
        while (reader.Read())
        {
            items.Add(new Registration
            {
                Id = reader.GetInt32("Id"),
                Name = reader.GetString("Name"),
                FatherName = reader.GetString("FatherName"),
                DateOfBirth = reader.IsDBNull("DateOfBirth")
                    ? null
                    : DateOnly.FromDateTime(reader.GetDateTime("DateOfBirth")),
                ContactNumber = reader.GetString("ContactNumber"),
                Address = reader.GetString("Address"),
                CreatedAtUtc = reader.GetDateTime("CreatedAtUtc")
            });
        }

        return items;
    }

    public bool Update(Registration registration)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        using var command = new MySqlCommand("""
            UPDATE Registrations
            SET Name = @name,
                FatherName = @fatherName,
                DateOfBirth = @dateOfBirth,
                ContactNumber = @contactNumber,
                Address = @address
            WHERE Id = @id;
            """, connection);

        command.Parameters.AddWithValue("@id", registration.Id);
        command.Parameters.AddWithValue("@name", registration.Name);
        command.Parameters.AddWithValue("@fatherName", registration.FatherName);
        command.Parameters.AddWithValue("@dateOfBirth", registration.DateOfBirth?.ToDateTime(TimeOnly.MinValue));
        command.Parameters.AddWithValue("@contactNumber", registration.ContactNumber);
        command.Parameters.AddWithValue("@address", registration.Address);

        return command.ExecuteNonQuery() == 1;
    }
}
