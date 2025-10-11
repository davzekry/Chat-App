using System.Security.Claims;
using ChatApp.Application.Common.Models;
using ChatApp.Application.Handlers.Authentication.Commands;
using ChatApp.Application.Handlers.Messages.Commands;
using ChatApp.Application.Handlers.Messages.DTOs;
using ChatApp.Application.Handlers.Messages.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessageController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        public async Task<CustomeResponse<DTO_GetAllMessagesByRoomIdQuery>> GetAllMessagesByRoomId([FromQuery]GetAllMessagesByRoomIdQuery request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost]
        public async Task<CustomeResponse<bool>> SendMessage(SendMessageCommand request)
        {
            var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (AppUserId == null)
                return CustomeResponse<bool>.Error("User Id Not Found!!");

            request.UserId = AppUserId;
            return await _mediator.Send(request);
        }


    }
}
