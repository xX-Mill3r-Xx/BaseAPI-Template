using BaseTemplate.Application.DTOs;
using FluentValidation;

namespace BaseTemplate.Application.Exceptions
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome é obrigatório.")
                .MaximumLength(200);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O preço não pode ser negativo.");
        }
    }
}
