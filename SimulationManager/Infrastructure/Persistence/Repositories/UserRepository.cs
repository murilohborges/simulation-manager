using Microsoft.EntityFrameworkCore;
using SimulationManager.Infrastructure.Persistence;
using SimulationManager.Domain.Entities;
using SimulationManager.Domain.Interfaces;

namespace SimulationManager.Infrastructure.Persistence.Repositories;
public class UserRepository: BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<User>> GetUsersWithFuelCompositionsAsync()
        => await _context.Users
            .Include(u => u.FuelCompositions)
            .AsNoTracking()
            .ToListAsync();
}