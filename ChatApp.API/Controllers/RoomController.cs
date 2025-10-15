using System.Security.Claims;
using ChatApp.Application.Common.Models;
using ChatApp.Application.Handlers.Rooms.Orchestrators;
using ChatApp.Application.Handlers.Users.DTOs;
using ChatApp.Domain.Entities;
using ChatApp.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatApp.Application.Features.Rooms.Orchestrators;
using ChatApp.Application.Handlers.Rooms.Queries;
using ChatApp.Application.Handlers.Rooms.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Chat_Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RoomController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        public async Task<CustomeResponse<bool>> CreatePrivateRoom([FromBody]CreatePrivateRoomOrchestrator roomRequest)
        {
            var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (AppUserId == null)
                return CustomeResponse<bool>.Error("User Id Not Found!!");
            
            roomRequest.CurrentUserId = AppUserId;
            return await _mediator.Send(roomRequest);
        }

        [HttpPost]
        public async Task<CustomeResponse<bool>> CreateGroupRoom([FromBody] CreateGroupRoomOrchestrator request)
        {
            var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (AppUserId == null)
                return CustomeResponse<bool>.Error("User Id Not Found!!");

            request.CurrentUserId = AppUserId;
            return await _mediator.Send(request);
        }

        [HttpGet]
        public async Task<CustomeResponse<List<DTO_GetAllRoomsByUserIdQuery>>> GetAllRoomsByUserId()
        {
            var AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (AppUserId == null)
                return CustomeResponse<List<DTO_GetAllRoomsByUserIdQuery>>.Error("User Id Not Found!!");

            return await _mediator.Send(new GetAllRoomsByUserIdQuery { UserId = AppUserId });
        }
    }
}
