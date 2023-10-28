using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Users.Create;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private const int NameMaxLength = 30;
    private const int EmailMaxLength = 30;
    private const int PasswordMaxLength = 16;

    public CreateUserCommandValidator()
    {
        RuleFor(cuc => cuc.Name).NotEmpty().WithMessage("Name should not be empty.");
        RuleFor(cuc => cuc.Name).MaximumLength(NameMaxLength).WithMessage($"Name should be shorter than {NameMaxLength} characters.");

        RuleFor(cuc => cuc.Email).NotEmpty().WithMessage("Email should not be empty.");
        RuleFor(cuc => cuc.Email).MaximumLength(EmailMaxLength).WithMessage($"Email should be shorter than {EmailMaxLength} characters.");
        RuleFor(cuc => cuc.Email).Must(email =>
        {
            var pattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            var match = Regex.Match(email, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.NonBacktracking);

            return match.Success;
        }).WithMessage("Email should comply with RFC2822 standards.");

        RuleFor(cuc => cuc.Password).NotEmpty().WithMessage("Password should not be empty.");
        RuleFor(cuc => cuc.Password).MaximumLength(NameMaxLength).WithMessage($"Password should be shorter than {PasswordMaxLength} characters.");
    }
}
