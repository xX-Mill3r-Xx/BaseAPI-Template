using BaseTemplate.Application.DTOs;
using BaseTemplate.Domain.Entities;
using BaseTemplate.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace BaseTemplate.IntegrationTests
{
    public class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public ProductsControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        private async Task<Guid> CriarCategoriaDeTesteAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var category = new Category("Categoria de Teste " + Guid.NewGuid());
            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return category.Id;
        }

        [Fact]
        public async Task PostProduct_ComDadosValidos_DeveRetornar201()
        {
            var categoryId = await CriarCategoriaDeTesteAsync();
            var dto = new CreateProductDto("Monitor", 899.90m, categoryId);

            var response = await _client.PostAsJsonAsync("/api/v1/products", dto);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<ProductDto>();
            created!.Name.Should().Be("Monitor");
            created.Price.Should().Be(899.90m);
        }

        [Fact]
        public async Task PostProduct_ComCategoriaInexistente_DeveRetornar404()
        {
            var dto = new CreateProductDto("Monitor", 899.90m, Guid.NewGuid());

            var response = await _client.PostAsJsonAsync("/api/v1/products", dto);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostProduct_ComPrecoNegativo_DeveRetornar400()
        {

            var categoryId = await CriarCategoriaDeTesteAsync();
            var dto = new CreateProductDto("Monitor", -10m, categoryId);

            var response = await _client.PostAsJsonAsync("/api/v1/products", dto);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetProductById_ComIdInexistente_DeveRetornar404()
        {
            var response = await _client.GetAsync($"/api/v1/products/{Guid.NewGuid()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
