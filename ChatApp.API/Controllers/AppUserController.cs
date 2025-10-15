using System.Security.Claims;
using ChatApp.Application.Common.Models;
using ChatApp.Application.Handlers.Users.DTOs;
using ChatApp.Application.Handlers.Users.Queries;
using ChatApp.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat_Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AppUserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppUserController(IMediator _mediator)
        {
            this._mediator = _mediator;
        }

        [HttpGet]
        public async Task<CustomeResponse<PageResult<DTO_GetAllUsersQuery>>> GetAllUsers()
        {
            var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (AppUserId == null)
                return CustomeResponse<PageResult<DTO_GetAllUsersQuery>>.Error("User Id Not Found!!");
            
            return await _mediator.Send(new GetAllUsersQuery { CurrentUserId = AppUserId });
        }
    }
}
