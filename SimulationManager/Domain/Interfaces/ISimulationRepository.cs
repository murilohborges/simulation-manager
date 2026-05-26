using SimulationManager.Domain.Entities;

namespace SimulationManager.Domain.Interfaces;

public interface ISimulationRepository : IRepository<Simulation>
{
    Task<Simulation?> GetByIdWithResultAsync(int id);
    Task UpdateStatusAsync(int id, string status);
    Task SaveResultAsync(SimulationResult result);
}
