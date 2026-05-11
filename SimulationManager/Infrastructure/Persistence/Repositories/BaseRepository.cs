using Microsoft.EntityFrameworkCore;
using SimulationManager.Domain.Interfaces;

namespace SimulationManager.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    protected BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _context.Set<T>().AsNoTracking().ToListAsync();

    public async Task<(IEnumerable<T> Data, int TotalRecords)> GetPagedAsync(
        int pageNumber,
        int pageSize)
    {
        var totalRecords = await _context.Set<T>().CountAsync();

        var data = await _context.Set<T>()
            .AsNoTracking()
            .OrderBy(e => EF.Property<int>(e, "Id"))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, totalRecords);
    }

    public async Task<T?> GetByIdAsync(int id)
        => await _context.Set<T>().FindAsync(id);

    public async Task<T> CreateAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        if (entity is IAuditableEntity auditableEntity)
            auditableEntity.UpdatedAt = DateTime.UtcNow;
        
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}