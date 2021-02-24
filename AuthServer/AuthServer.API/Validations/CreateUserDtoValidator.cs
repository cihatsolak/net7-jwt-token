using AuthServer.Core.DTOs;
using FluentValidation;

namespace AuthServer.API.Validations
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(p => p.Email).NotEmpty().WithMessage("Email is required")
                                 .EmailAddress().WithMessage("Email is wrong.");

            RuleFor(p => p.Password).NotEmpty().WithMessage("Password is required.");
            RuleFor(p => p.UserName).NotEmpty().WithMessage("Password is required.");
        }
    }
}
