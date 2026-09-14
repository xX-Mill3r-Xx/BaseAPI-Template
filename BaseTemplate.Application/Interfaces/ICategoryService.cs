using BaseTemplate.Application.DTOs;

namespace BaseTemplate.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default);
        Task UpdateAsync(Guid id, UpdateCategoryDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
