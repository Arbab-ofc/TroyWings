using MySqlConnector;
using TroyWingsApp.Models;

namespace TroyWingsApp.Data;

public interface IRegistrationRepository
{
    Task EnsureDatabaseSetupAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(Registration registration, CancellationToken cancellationToken = default);
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

    public async Task EnsureDatabaseSetupAsync(CancellationToken cancellationToken = default)
    {
        var builder = new MySqlConnectionStringBuilder(_connectionString);
        var databaseName = builder.Database;
        var sanitizedDatabaseName = databaseName.Replace("`", "``", StringComparison.Ordinal);

        if (!string.IsNullOrWhiteSpace(databaseName))
        {
            var adminBuilder = new MySqlConnectionStringBuilder(_connectionString)
            {
                Database = string.Empty
            };

            await using var adminConnection = new MySqlConnection(adminBuilder.ConnectionString);
            await adminConnection.OpenAsync(cancellationToken);

            var createDbCommand = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS `{sanitizedDatabaseName}`;", adminConnection);
            await createDbCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string createTableSql = """
            CREATE TABLE IF NOT EXISTS Registrations (
                Id INT AUTO_INCREMENT PRIMARY KEY,
                Name VARCHAR(80) NOT NULL,
                FatherName VARCHAR(80) NOT NULL,
                DateOfBirth DATE NOT NULL,
                ContactNumber VARCHAR(14) NOT NULL,
                Address VARCHAR(180) NOT NULL,
                CreatedAtUtc DATETIME NOT NULL
            );
            """;

        await using var command = new MySqlCommand(createTableSql, connection);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task SaveAsync(Registration registration, CancellationToken cancellationToken = default)
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string insertSql = """
            INSERT INTO Registrations
                (Name, FatherName, DateOfBirth, ContactNumber, Address, CreatedAtUtc)
            VALUES
                (@Name, @FatherName, @DateOfBirth, @ContactNumber, @Address, @CreatedAtUtc);
            """;

        await using var command = new MySqlCommand(insertSql, connection);
        command.Parameters.AddWithValue("@Name", registration.Name);
        command.Parameters.AddWithValue("@FatherName", registration.FatherName);
        command.Parameters.AddWithValue("@DateOfBirth", registration.DateOfBirth?.ToDateTime(TimeOnly.MinValue));
        command.Parameters.AddWithValue("@ContactNumber", registration.ContactNumber);
        command.Parameters.AddWithValue("@Address", registration.Address);
        command.Parameters.AddWithValue("@CreatedAtUtc", registration.CreatedAtUtc);

        var affected = await command.ExecuteNonQueryAsync(cancellationToken);
        if (affected != 1)
        {
            _logger.LogWarning("Unexpected rows affected when inserting registration: {Count}", affected);
        }
    }
}
