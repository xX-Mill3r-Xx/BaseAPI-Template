using BaseTemplate.Domain.Entities;

namespace BaseTemplate.Domain.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<IReadOnlyList<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
    }
}
