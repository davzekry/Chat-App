using ChatApp.Application.Handlers.Authentication.Commands;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ChatApp.Application.Handlers.Authentication.Validators
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
                //.MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                //.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                //.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                //.Matches("[0-9]").WithMessage("Password must contain at least one number.")
                //.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.UserImage)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("User image must not exceed 5MB.");
            RuleFor(x => x.UserImage)
                .Must(file =>
                {
                    if (file == null) return true;

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };
                    var extension = Path.GetExtension(file.FileName)?.ToLower();

                    return allowedExtensions.Contains(extension);
                })
                .WithMessage("Only .jpg, .jpeg, .png, and .gif files are allowed.");
        }
    }
}
