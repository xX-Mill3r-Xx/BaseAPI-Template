using BaseTemplate.Application.DTOs;
using BaseTemplate.Domain.Entities;

namespace BaseTemplate.Application.Mappings
{
    public static class ProductMappings
    {
        public static ProductDto ToDto(this Product product) =>
            new(
                product.Id,
                product.Name,
                product.Price,
                product.CategoryId,
                product.Category?.Name ?? string.Empty
            );
    }
}
