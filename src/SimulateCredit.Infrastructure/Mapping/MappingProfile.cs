using AutoMapper;
using SimulateCredit.Domain.Entities;
using SimulateCredit.Infrastructure.Adapters.Persistence.Mongo;

namespace SimulateCredit.Infrastructure.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Simulation, SimulationDocument>();
    }
}
