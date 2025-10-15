using ChatApp.Application.Common.Models;
using ChatApp.Application.Handlers.RoomMembers.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat_Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RoomMemberController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomMemberController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        public Task<CustomeResponse<bool>> AddRoomMember(AddRoomMemberCommand request)
        {
            return _mediator.Send(request);
        }
    }
}
