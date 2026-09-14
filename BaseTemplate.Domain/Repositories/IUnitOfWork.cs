namespace BaseTemplate.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
