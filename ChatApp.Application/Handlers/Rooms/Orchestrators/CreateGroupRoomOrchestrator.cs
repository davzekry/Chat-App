using ChatApp.Application.Common.Models;
using ChatApp.Application.Handlers.RoomMembers.Commands;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.Features.Rooms.Orchestrators
{
    public class CreateGroupRoomOrchestrator : IRequest<CustomeResponse<bool>>
    {
        public string CurrentUserId { get; set; }
        public List<string> MemberIds { get; set; } = new();
        public string GroupName { get; set; }
        public string? ImagePath { get; set; }
    }

    public class CreateGroupRoomOrchestratorHandler : IRequestHandler<CreateGroupRoomOrchestrator, CustomeResponse<bool>>
    {
        private readonly IGenericRepository<Room> _roomRepository;
        private readonly IMediator _mediator;

        public CreateGroupRoomOrchestratorHandler(
            IGenericRepository<Room> roomRepository,
            IMediator mediator)
        {
            _roomRepository = roomRepository;
            _mediator = mediator;
        }

        public async Task<CustomeResponse<bool>> Handle(CreateGroupRoomOrchestrator request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.GroupName))
                return CustomeResponse<bool>.Fail("Group name is required");

            // Step 1: Create the group room
            var room = new Room
            {
                RoomName = request.GroupName,
                RoomType = RoomType.Group,
                CreatedByUserId = request.CurrentUserId,
                ImagePath = request.ImagePath
            };

            await _roomRepository.AddAsync(room);
            await _roomRepository.SaveChangesAsync();

            // Step 2: Add the creator + provided members
            var allMembers = request.MemberIds ?? new List<string>();
            if (!allMembers.Contains(request.CurrentUserId))
                allMembers.Add(request.CurrentUserId);

            var addMembersCommand = new AddRoomMemberCommand
            {
                RoomId = room.Id,
                UserIds = allMembers.Distinct().ToList()
            };

            var result = await _mediator.Send(addMembersCommand, cancellationToken);
            if (result.Status != ResponseStatus.Success)
                return CustomeResponse<bool>.Fail("Group created but failed to add members");

            return CustomeResponse<bool>.Success(true, "Group room created successfully");
        }
    }
}
