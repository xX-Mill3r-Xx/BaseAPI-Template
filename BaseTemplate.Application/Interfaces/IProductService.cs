using BaseTemplate.Application.DTOs;

namespace BaseTemplate.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
        Task UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
