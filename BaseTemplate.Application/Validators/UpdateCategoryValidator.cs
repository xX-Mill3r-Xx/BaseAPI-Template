using BaseTemplate.Application.DTOs;
using FluentValidation;

namespace BaseTemplate.Application.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty()
                .WithMessage("O nome da categoria é obrigatório.")
                .MaximumLength(150);
        }
    }
}
