using AutoMapper;
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

    public MongoSimulationRepository(IOptions<MongoDbSettings> settings, IMapper mapper)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.Database);
        _collection = database.GetCollection<SimulationDocument>("simulations");

        _mapper = mapper;
    }

    public Task SaveAsync(Simulation simulation)
    {
        var document = _mapper.Map<SimulationDocument>(simulation);

        return _collection.InsertOneAsync(document);
    }
}
