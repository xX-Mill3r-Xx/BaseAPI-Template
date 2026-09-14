using BaseTemplate.Application.DTOs;
using BaseTemplate.Application.Exceptions;
using BaseTemplate.Application.Interfaces;
using BaseTemplate.Application.Mappings;
using BaseTemplate.Domain.Entities;
using BaseTemplate.Domain.Repositories;
using FluentValidation;

namespace BaseTemplate.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCategoryDto> _createValidator;
        private readonly IValidator<UpdateCategoryDto> _updateValidator;

        public CategoryService(
        IUnitOfWork unitOfWork,
        IValidator<CreateCategoryDto> createValidator,
        IValidator<UpdateCategoryDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);
            return category?.ToDto();
        }

        public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(cancellationToken);
            return categories.Select(c => c.ToDto()).ToList();
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Exceptions.ValidationException(validationResult.Errors);

            var category = new Category(dto.name);

            await _unitOfWork.Categories.AddAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return category.ToDto();
        }

        public async Task UpdateAsync(Guid id, UpdateCategoryDto dto, CancellationToken cancellationToken = default)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
                throw new Exceptions.ValidationException(validationResult.Errors);

            var category = await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken)
                ?? throw NotFoundException.For<Category>(id);

            category.SetName(dto.name);

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken)
                ?? throw NotFoundException.For<Category>(id);

            var products = await _unitOfWork.Products.GetByCategoryIdAsync(id, cancellationToken);
            if (products.Any())
                throw new BusinessRuleException(
                    $"Não é possível excluir a categoria '{category.Name}' pois existem {products.Count} produto(s) vinculado(s) a ela.");

            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
