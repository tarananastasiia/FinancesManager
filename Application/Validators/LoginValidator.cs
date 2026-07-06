using Application.Auth.Commands.Login;
using FluentValidation;

namespace Application.Validators
{
    public class LoginCommandValidator
        : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Model.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Model.Password)
                .NotEmpty();
        }
    }
}
