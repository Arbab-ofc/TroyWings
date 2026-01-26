using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
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

public class MongoRegistrationRepository : IRegistrationRepository
{
    private const string RegistrationsCollection = "Registrations";
    private const string CountersCollection = "Counters";
    private const string CounterId = "registrations";
    private readonly IMongoCollection<RegistrationDocument> _registrations;
    private readonly IMongoCollection<CounterDocument> _counters;
    private readonly ILogger<MongoRegistrationRepository> _logger;

    public MongoRegistrationRepository(IConfiguration configuration, ILogger<MongoRegistrationRepository> logger)
    {
        var connectionString = configuration["Mongo:ConnectionString"]
            ?? throw new InvalidOperationException("Mongo connection string 'Mongo:ConnectionString' not found.");
        var databaseName = configuration["Mongo:Database"]
            ?? throw new InvalidOperationException("Mongo database name 'Mongo:Database' not found.");

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _registrations = database.GetCollection<RegistrationDocument>(RegistrationsCollection);
        _counters = database.GetCollection<CounterDocument>(CountersCollection);
        _logger = logger;
    }

    public void EnsureDatabaseSetup()
    {
        try
        {
            var indexKeys = Builders<RegistrationDocument>.IndexKeys.Descending(x => x.CreatedAtUtc);
            _registrations.Indexes.CreateOne(new CreateIndexModel<RegistrationDocument>(indexKeys));

            _counters.UpdateOne(
                x => x.Id == CounterId,
                Builders<CounterDocument>.Update.SetOnInsert(x => x.Value, 0),
                new UpdateOptions { IsUpsert = true });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to ensure MongoDB setup.");
        }
    }

    public void Save(Registration registration)
    {
        if (registration == null)
        {
            throw new ArgumentNullException(nameof(registration));
        }

        var nextId = GetNextId();
        registration.Id = nextId;

        var document = MapToDocument(registration);
        _registrations.InsertOne(document);
    }

    public PagedResult<Registration> GetPage(int page, int pageSize)
    {
        var safePage = Math.Max(1, page);
        var safePageSize = Math.Clamp(pageSize, 1, 24);

        var totalCount = (int)_registrations.CountDocuments(FilterDefinition<RegistrationDocument>.Empty);
        var skip = (safePage - 1) * safePageSize;

        var items = _registrations.Find(FilterDefinition<RegistrationDocument>.Empty)
            .SortByDescending(x => x.CreatedAtUtc)
            .Skip(skip)
            .Limit(safePageSize)
            .ToList()
            .Select(MapToModel)
            .ToList();

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
        return _registrations.Find(FilterDefinition<RegistrationDocument>.Empty)
            .SortByDescending(x => x.CreatedAtUtc)
            .ToList()
            .Select(MapToModel)
            .ToList();
    }

    public bool Update(Registration registration)
    {
        if (registration == null)
        {
            throw new ArgumentNullException(nameof(registration));
        }

        var update = Builders<RegistrationDocument>.Update
            .Set(x => x.Name, registration.Name)
            .Set(x => x.FatherName, registration.FatherName)
            .Set(x => x.DateOfBirth, registration.DateOfBirth?.ToDateTime(TimeOnly.MinValue))
            .Set(x => x.ContactNumber, registration.ContactNumber)
            .Set(x => x.Address, registration.Address);

        var result = _registrations.UpdateOne(x => x.Id == registration.Id, update);
        return result.ModifiedCount == 1;
    }

    private int GetNextId()
    {
        var update = Builders<CounterDocument>.Update.Inc(x => x.Value, 1);
        var options = new FindOneAndUpdateOptions<CounterDocument, CounterDocument>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        var counter = _counters.FindOneAndUpdate<CounterDocument, CounterDocument>(
            x => x.Id == CounterId,
            update,
            options);
        return counter.Value;
    }

    private static RegistrationDocument MapToDocument(Registration registration)
    {
        return new RegistrationDocument
        {
            Id = registration.Id,
            Name = registration.Name,
            FatherName = registration.FatherName,
            DateOfBirth = registration.DateOfBirth?.ToDateTime(TimeOnly.MinValue),
            ContactNumber = registration.ContactNumber,
            Address = registration.Address,
            CreatedAtUtc = registration.CreatedAtUtc
        };
    }

    private static Registration MapToModel(RegistrationDocument document)
    {
        return new Registration
        {
            Id = document.Id,
            Name = document.Name,
            FatherName = document.FatherName,
            DateOfBirth = document.DateOfBirth.HasValue
                ? DateOnly.FromDateTime(document.DateOfBirth.Value)
                : null,
            ContactNumber = document.ContactNumber,
            Address = document.Address,
            CreatedAtUtc = document.CreatedAtUtc
        };
    }

    private sealed class RegistrationDocument
    {
        [BsonId]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string FatherName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public string ContactNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; }
    }

    private sealed class CounterDocument
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;

        public int Value { get; set; }
    }
}
