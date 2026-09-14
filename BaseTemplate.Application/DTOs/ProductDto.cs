namespace BaseTemplate.Application.DTOs
{
    public record ProductDto(
        Guid Id,
        string Name,
        decimal Price,
        Guid CategoryId,
        string CategoryName);
}
