namespace BaseTemplate.Application.DTOs
{
    public record CreateProductDto(
        string Name,
        decimal Price,
        Guid CategoryId);
}
