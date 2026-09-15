using BaseTemplate.Domain.Entities;
using FluentAssertions;

namespace BaseTemplate.UnitTests.Domain
{
    public class ProductTests
    {
        [Fact]
        public void CriarProduto_ComDadosValidos_DeveCriarComSucesso()
        {
            var categoryId = Guid.NewGuid();
            var product = new Product("Notebook", 3500m, categoryId);

            product.Name.Should().Be("Notebook");
            product.Price.Should().Be(3500m);
            product.CategoryId.Should().Be(categoryId);
            product.Id.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100.50)]
        public void CriarProduto_ComPrecoNegativo_DeveLancarExcecao(decimal precoInvalido)
        {
            var categoryId = Guid.NewGuid();
            var act = () => new Product("Notebook", precoInvalido, categoryId);

            act.Should().Throw<ArgumentException>()
                .WithoutMessage("*preço não pode ser negativo*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void CriarProduto_ComNomeInvalido_DeveLancarExcecao(string? nomeInvalido)
        {
            var categoryId = Guid.NewGuid();
            var act = () => new Product(nomeInvalido!, 100m, categoryId);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*nome do produto não pode ser vazio*");
        }

        [Fact]
        public void AtualizarPreco_ComValorValido_DeveAtualizar()
        {
            var product = new Product("Mouse", 50m, Guid.NewGuid());
            product.SetPrice(75m);

            product.Price.Should().Be(75m);
        }
    }
}
