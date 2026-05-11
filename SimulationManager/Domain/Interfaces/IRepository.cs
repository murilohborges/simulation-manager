namespace SimulationManager.Domain.Interfaces
{
    public interface IRepository<T> where T: class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<(IEnumerable<T> Data, int TotalRecords)> GetPagedAsync(
            int pageNumber,
            int pageSize);
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
