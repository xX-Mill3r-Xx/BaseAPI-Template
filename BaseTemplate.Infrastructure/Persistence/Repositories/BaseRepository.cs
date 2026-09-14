using BaseTemplate.Domain.Entities;
using BaseTemplate.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BaseTemplate.Infrastructure.Persistence.Repositories
{
    public class BaseRepository<T>
        : IBaseRepository<T>
        where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dBSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dBSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            await _dBSet.FindAsync(new object[] { id }, cancellationToken);

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _dBSet.AsNoTracking().ToListAsync(cancellationToken);

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
            await _dBSet.AddAsync(entity, cancellationToken);

        public void Update(T entity) => _dBSet.Update(entity);

        public void Remove(T entity) => _dBSet.Remove(entity);
    }
}
