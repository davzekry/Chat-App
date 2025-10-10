using ChatApp.Application.Common.Models;
using ChatApp.Application.Handles.Authentication.DTOs;
using ChatApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EGRide.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<CustomeResponse<LoginJwtResponseDTO>>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class LoginHandler : IRequestHandler<LoginCommand, CustomeResponse<LoginJwtResponseDTO>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration config;

        public LoginHandler(UserManager<AppUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        public async Task<CustomeResponse<LoginJwtResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            AppUser? user = await userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                bool result = await userManager.CheckPasswordAsync(user, request.Password);

                if (result)
                {
                    string JtiString = Guid.NewGuid().ToString();


                    List<Claim> claimList = new List<Claim>();
                    claimList.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    claimList.Add(new Claim(ClaimTypes.Name, user.UserName));
                    claimList.Add(new Claim(ClaimTypes.Email, user.Email));
                    claimList.Add(new Claim(JwtRegisteredClaimNames.Jti, JtiString));
                    
                    
                    SecurityKey securityKeyObj = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));

                    SigningCredentials signingCredentialsObj = new(securityKeyObj, SecurityAlgorithms.HmacSha256);

                    var ExpireDate = DateTime.Now.AddDays(int.Parse(config["Jwt:ExpireDate"]));
                    JwtSecurityToken mytoken = new JwtSecurityToken(                        
                        expires: ExpireDate,
                        claims: claimList,
                        signingCredentials: signingCredentialsObj
                        );

                    return CustomeResponse<LoginJwtResponseDTO>.Success(new LoginJwtResponseDTO
                    {
                        ExpireDate = ExpireDate,
                        Token = new JwtSecurityTokenHandler().WriteToken(mytoken)
                    });
                }
            }
            return CustomeResponse<LoginJwtResponseDTO>.Fail("Invalid Account", ResponseStatus.NotFound);
        }
    }
}