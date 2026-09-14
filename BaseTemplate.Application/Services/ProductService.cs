using BaseTemplate.Application.DTOs;
using BaseTemplate.Application.Exceptions;
using BaseTemplate.Application.Interfaces;
using BaseTemplate.Application.Mappings;
using BaseTemplate.Domain.Entities;
using BaseTemplate.Domain.Repositories;
using FluentValidation;

namespace BaseTemplate.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateProductDto> _createValidator;
        private readonly IValidator<UpdateProductDto> _updateValidator;

        public ProductService(
            IUnitOfWork unitOfWork,
            IValidator<CreateProductDto> createValidator,
            IValidator<UpdateProductDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id, cancellationToken);
            return product?.ToDto();
        }

        public async Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var products = await _unitOfWork.Products.GetAllAsync(cancellationToken);
            return products.Select(p => p.ToDto()).ToList();
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Exceptions.ValidationException(validationResult.Errors);

            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId, cancellationToken)
                ?? throw NotFoundException.For<Category>(dto.CategoryId);

            var product = new Product(dto.Name, dto.Price, dto.CategoryId);

            await _unitOfWork.Products.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            product = await _unitOfWork.Products.GetByIdAsync(product.Id, cancellationToken);
            return product!.ToDto();
        }

        public async Task UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Exceptions.ValidationException(validationResult.Errors);

            var product = await _unitOfWork.Products.GetByIdAsync(id, cancellationToken)
                ?? throw NotFoundException.For<Product>(id);

            product.SetName(dto.Name);
            product.SetPrice(dto.Price);
            product.UpdateAuditDate();

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id, cancellationToken)
                ?? throw NotFoundException.For<Product>(id);

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
