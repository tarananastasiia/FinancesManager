using Application.Auth.Commands.Register;
using FluentValidation;

namespace Application.Validators
{
    public class RegisterCommandValidator
        : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Model.FullName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Model.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Model.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]")
                .WithMessage("Password must contain uppercase letter.")
                .Matches("[a-z]")
                .WithMessage("Password must contain lowercase letter.")
                .Matches("[0-9]")
                .WithMessage("Password must contain number.");
        }
    }
}
