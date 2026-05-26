using Microsoft.EntityFrameworkCore;
using SimulationManager.Domain.Entities;
using SimulationManager.Domain.Interfaces;

namespace SimulationManager.Infrastructure.Persistence.Repositories;

public class SimulationRepository: BaseRepository<Simulation>, ISimulationRepository
{
    public SimulationRepository(AppDbContext context) : base(context){}

    // Override GetPagedAsync to include the SimulationResult
    public override async Task<(IEnumerable<Simulation> Data, int TotalRecords)> GetPagedAsync(
        int pageNumber,
        int pageSize)
    {
        var totalRecords = await _context.Simulations.CountAsync();

        var data = await _context.Simulations
            .Include(s => s.SimulationResult)
            .AsNoTracking()
            .OrderBy(e => EF.Property<int>(e, "Id"))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, totalRecords);
    }

    public async Task<Simulation?> GetByIdWithResultAsync(int id)
        => await _context.Simulations
            .Include(s => s.SimulationResult)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task UpdateStatusAsync(int id, string status)
    {
        var simulation = await _context.Simulations.FindAsync(id);
        if (simulation is null) return;

        simulation.Status = status;
        await _context.SaveChangesAsync();
    }

    public async Task SaveResultAsync(SimulationResult result)
    {
        _context.SimulationResults.Add(result);
        await _context.SaveChangesAsync();
    }
}
