using BaseTemplate.Application.DTOs;
using BaseTemplate.Domain.Entities;

namespace BaseTemplate.Application.Mappings
{
    public static class CategoryMappings
    {
        public static CategoryDto ToDto(this Category category) =>
            new(category.Id, category.Name);
    }
}
