using SimulationManager.Infrastructure.Persistence;
using SimulationManager.Domain.Entities;
using SimulationManager.Domain.Interfaces;

namespace SimulationManager.Infrastructure.Persistence.Repositories;
public class FuelCompositionRepository
    : BaseRepository<FuelComposition>, IFuelCompositionRepository
{
    public FuelCompositionRepository(AppDbContext context) : base(context) { }
}