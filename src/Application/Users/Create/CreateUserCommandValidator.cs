using Application.Messages;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Users.Create;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public readonly int NameMaxLength = 30;
    public readonly int EmailMaxLength = 30;
    public readonly int PasswordMaxLength = 16;

    public CreateUserCommandValidator()
    {
        RuleFor(cuc => cuc.Name).NotEmpty().WithMessage(GenericValidatorMessages.ShouldNotBeEmptyMessage());
        RuleFor(cuc => cuc.Name).MaximumLength(NameMaxLength).WithMessage(GenericValidatorMessages.ShouldBeShorterThanMessage(NameMaxLength));

        RuleFor(cuc => cuc.Email).NotEmpty().WithMessage(GenericValidatorMessages.ShouldNotBeEmptyMessage());
        RuleFor(cuc => cuc.Email).MaximumLength(EmailMaxLength).WithMessage(GenericValidatorMessages.ShouldBeShorterThanMessage(EmailMaxLength));
        RuleFor(cuc => cuc.Email).Must(email =>
        {
            var pattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            var match = Regex.Match(email, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.NonBacktracking);

            return match.Success;
        }).WithMessage(GenericValidatorMessages.ShouldComplyWithRFC2822StandardsMessage());

        RuleFor(cuc => cuc.Password).NotEmpty().WithMessage(GenericValidatorMessages.ShouldNotBeEmptyMessage());
        RuleFor(cuc => cuc.Password).MaximumLength(PasswordMaxLength).WithMessage(GenericValidatorMessages.ShouldBeShorterThanMessage(PasswordMaxLength));
    }
}
