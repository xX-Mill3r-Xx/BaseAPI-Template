using BaseTemplate.Application.DTOs;
using BaseTemplate.Application.Exceptions;
using FluentValidation.TestHelper;

namespace BaseTemplate.UnitTests.Application
{
    public class CreateProductValidatorTests
    {
        private readonly CreateProductValidator _validator = new();

        [Fact]
        public void Validar_ComNomeVazio_DeveRetornarErro()
        {
            var dto = new CreateProductDto("", 100m, Guid.NewGuid());

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validar_ComPrecoNegativo_DeveRetornarErro()
        {
            var dto = new CreateProductDto("Produto", -1m, Guid.NewGuid());

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Validar_ComDadosValidos_NaoDeveRetornarErro()
        {
            var dto = new CreateProductDto("Produto", 100m, Guid.NewGuid());

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
