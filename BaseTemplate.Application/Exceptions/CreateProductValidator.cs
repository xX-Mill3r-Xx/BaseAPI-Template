using BaseTemplate.Application.DTOs;
using FluentValidation;

namespace BaseTemplate.Application.Exceptions
{
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório")
                .MaximumLength(200);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O preço não pode ser negativo.");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("A categoria é obrigatória.");
        }
    }
}
