using BaseTemplate.Domain.Entities;
using BaseTemplate.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BaseTemplate.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default) =>
            await _dBSet.AsNoTracking()
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
    }
}
