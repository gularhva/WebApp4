using FluentValidation;
using WebApp4.DTOs.School_DTOs;

namespace WebApp4.Validations
{
    public class SchoolCreateDTOValidator:AbstractValidator<SchoolCreateDTO>
    {
        public SchoolCreateDTOValidator()
        {
            RuleFor(dto => dto.Number)
            .NotEmpty().WithMessage("School number is required.")
            .GreaterThan(0).WithMessage("School number must be greater than zero.");

            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("School name is required.")
                .MaximumLength(100).WithMessage("School name must not exceed 100 characters.");
        }
    }
}
