using Microsoft.EntityFrameworkCore;
using PayrollSystem.Application.Abstractions;
using PayrollSystem.Infrastructure.Persistence;

namespace PayrollSystem.Infrastructure.Repositories;

public sealed class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly AppDbContext _db;

    public EfRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default)
        => _db.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);

    public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _db.Set<TEntity>().FindAsync([id], cancellationToken).AsTask();

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _db.Set<TEntity>().Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _db.Set<TEntity>().Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _db.Set<TEntity>().Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
