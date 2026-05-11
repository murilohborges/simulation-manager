using SimulationManager.Domain.Entities;

namespace SimulationManager.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetUsersWithFuelCompositionsAsync();
    }
}
