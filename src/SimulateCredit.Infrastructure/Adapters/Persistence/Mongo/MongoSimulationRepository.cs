using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SimulateCredit.Application.Ports.Outgoing;
using SimulateCredit.Domain.Entities;
using SimulateCredit.Infrastructure.Settings;

namespace SimulateCredit.Infrastructure.Adapters.Persistence.Mongo;

public sealed class MongoSimulationRepository : ISimulationRepository
{
    private readonly IMongoCollection<SimulationDocument> _collection;
    private readonly IMapper _mapper;
    private readonly ILogger<MongoSimulationRepository> _logger;

    public MongoSimulationRepository(IOptions<MongoDbSettings> settings, IMapper mapper, ILogger<MongoSimulationRepository> logger)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.Database);
        _collection = database.GetCollection<SimulationDocument>("simulations");

        _mapper = mapper;
        _logger = logger;
    }

    public async Task SaveAsync(Simulation simulation)
    {
        try
        {
            var document = _mapper.Map<SimulationDocument>(simulation);
            await _collection.InsertOneAsync(document);
            _logger.LogInformation("Simulation stored for {Email}", simulation.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error storing simulation for {Email}", simulation.Email);
            throw;
        }
    }
}