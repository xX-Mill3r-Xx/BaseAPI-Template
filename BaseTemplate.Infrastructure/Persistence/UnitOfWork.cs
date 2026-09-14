using BaseTemplate.Domain.Repositories;
using BaseTemplate.Infrastructure.Persistence.Repositories;

namespace BaseTemplate.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IProductRepository? _products;
        private ICategoryRepository? _category;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProductRepository Products => _products ??= new ProductRepository(_context);

        public ICategoryRepository Categories => _category ??= new CategoryRepository(_context);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _context.SaveChangesAsync(cancellationToken);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
