using System.Linq.Expressions;
using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LemonLaw.Infrastructure.Repositories;

public class GenericRepository<T>(LemonLawAPIDbContext context) : IGenericRepository<T> where T : class
{
    protected readonly LemonLawAPIDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.Where(predicate).ToListAsync();

    public async Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.FirstOrDefaultAsync(predicate);

    public async Task<T?> FindOneIncludingDeletedAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(predicate);

    public async Task AddAsync(T entity) =>
        await _dbSet.AddAsync(entity);

    public void Update(T entity) =>
        _dbSet.Update(entity);

    public void Delete(T entity)
    {
        // Soft delete if entity supports it
        if (entity is AuditDetails baseEntity)
        {
            baseEntity.IsDeleted = true;
            baseEntity.ModifiedDate = DateTime.UtcNow;
            _dbSet.Update(entity);
        }
        else
        {
            _dbSet.Remove(entity);
        }
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
