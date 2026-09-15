using BaseTemplate.Application.DTOs;
using BaseTemplate.Application.Exceptions;
using BaseTemplate.Application.Services;
using BaseTemplate.Domain.Entities;
using BaseTemplate.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using ValidationException = BaseTemplate.Application.Exceptions.ValidationException;

namespace BaseTemplate.UnitTests.Application
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IValidator<CreateProductDto>> _createValidatorMock = new();
        private readonly Mock<IValidator<UpdateProductDto>> _updateValidatorMock = new();
        private readonly ProductService _sut; 

        public ProductServiceTests()
        {
            _sut = new ProductService(
                _unitOfWorkMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ComCategoriaInexistente_DeveLancarNotFoundException()
        {
            var dto = new CreateProductDto("Teclado", 150m, Guid.NewGuid());

            _createValidatorMock
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult()); 

            _unitOfWorkMock
                .Setup(u => u.Categories.GetByIdAsync(dto.CategoryId, default))
                .ReturnsAsync((Category?)null); 

            var act = async () => await _sut.CreateAsync(dto);

            await act.Should().ThrowAsync<NotFoundException>();
            _unitOfWorkMock.Verify(u => u.Products.AddAsync(It.IsAny<Product>(), default), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ComDadosValidos_DeveCriarProduto()
        {
            var category = new Category("Eletrônicos");
            var dto = new CreateProductDto("Teclado", 150m, category.Id);

            _createValidatorMock
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _unitOfWorkMock
                .Setup(u => u.Categories.GetByIdAsync(dto.CategoryId, default))
                .ReturnsAsync(category);

            _unitOfWorkMock
                .Setup(u => u.Products.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync((Guid id, CancellationToken _) => new Product(dto.Name, dto.Price, dto.CategoryId));

            var result = await _sut.CreateAsync(dto);

            result.Name.Should().Be("Teclado");
            result.Price.Should().Be(150m);

            _unitOfWorkMock.Verify(u => u.Products.AddAsync(It.IsAny<Product>(), default), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ComDadosInvalidos_DeveLancarValidationException()
        {
            var dto = new CreateProductDto("", -10m, Guid.NewGuid());

            var falhas = new List<ValidationFailure>
        {
            new("Name", "O nome é obrigatório."),
            new("Price", "O preço não pode ser negativo.")
        };

            _createValidatorMock
                .Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult(falhas));

            var act = async () => await _sut.CreateAsync(dto);

            var exception = await act.Should().ThrowAsync<ValidationException>();
            exception.Which.Errors.Should().ContainKey("Name");
            exception.Which.Errors.Should().ContainKey("Price");

            _unitOfWorkMock.Verify(u => u.Categories.GetByIdAsync(It.IsAny<Guid>(), default), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ComProdutoInexistente_DeveLancarNotFoundException()
        {
            var id = Guid.NewGuid();
            _unitOfWorkMock
                .Setup(u => u.Products.GetByIdAsync(id, default))
                .ReturnsAsync((Product?)null);

            var act = async () => await _sut.DeleteAsync(id);
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
