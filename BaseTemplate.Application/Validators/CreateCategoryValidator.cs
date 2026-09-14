using BaseTemplate.Application.DTOs;
using FluentValidation;

namespace BaseTemplate.Application.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty()
                .WithMessage("O nome da categoria é obrigatório.")
                .MaximumLength(150);
        }
    }
}
