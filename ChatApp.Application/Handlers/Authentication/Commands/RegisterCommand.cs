using ChatApp.Application.Common.Models;
using ChatApp.Application.Handlers.Authentication.Validators;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Helper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Application.Handlers.Authentication.Commands
{
    public class RegisterCommand : IRequest<CustomeResponse<bool>>
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public IFormFile? UserImage { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, CustomeResponse<bool>>
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterCommandHandler(UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<CustomeResponse<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new RegisterCommandValidator();
                ValidationResult validationResult = await validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    // Return a failed response with all error messages
                    string errors = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return CustomeResponse<bool>.Fail(errors);
                }



                string UserEmail = request.Email.ToLower();

                if (await _userManager.FindByEmailAsync(request.Email) != null)
                {
                    return CustomeResponse<bool>.Error("Email already exists. Please use a different email address.");
                }

                // creating and saving Image Path
                string userImageURL = string.Empty;

                if (request?.UserImage?.Length > 0)
                {
                    userImageURL = await FileHelper.UploadFileAsync(request.UserImage);
                }

                AppUser user = new()
                {
                    UserName = request?.UserName,
                    Email = request?.Email,
                    ImagePath = userImageURL,
                };

                if (await _userManager.FindByEmailAsync(request.Email) != null)
                {
                    return CustomeResponse<bool>.Error("Email already exists. Please use a different email address.");
                }
                // IdentityResult used to know if user creation is a success or not
                IdentityResult result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return CustomeResponse<bool>.Error(string.Join(",", result.Errors.Select(e => e.Description)));
                }

                return CustomeResponse<bool>.Success(true, "Account Created Successfully");

            }
            catch (Exception ex)
            {
                return CustomeResponse<bool>.Error($"Failed To Register Your Account! \n{ex}");
            }
        }
    }
}
