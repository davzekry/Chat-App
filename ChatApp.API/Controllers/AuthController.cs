using Azure;
using ChatApp.Application.Common.Models;
using ChatApp.Application.Handlers.Authentication.Commands;
using ChatApp.Application.Handles.Authentication.DTOs;
using ChatApp.Domain.Entities;
using EGRide.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chat_Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(IMediator _mediator, SignInManager<AppUser> signInManager)
        {
            this._mediator = _mediator;
            this._signInManager = signInManager;
        }

        [HttpPost]
        public async Task<CustomeResponse<bool>> Register([FromForm] RegisterCommand request)
        {

            return await _mediator.Send(request);
        }
        
        [HttpPost]
        public async Task<CustomeResponse<LoginJwtResponseDTO>> Login(LoginCommand loginRequest)
        {
            return await _mediator.Send(loginRequest);
        }

        [HttpPost]
        [Authorize]
        public async Task<CustomeResponse<bool>> Logout()
        {
            await _signInManager.SignOutAsync();
            return new CustomeResponse<bool>
            {
                Data = true,
                Status = ResponseStatus.Success,
                Message = "User logged out successfully"
            };
        }
    }
}
