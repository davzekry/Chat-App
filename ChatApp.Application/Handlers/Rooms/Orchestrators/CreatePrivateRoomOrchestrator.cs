using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ChatApp.Application.Common.Models;
using ChatApp.Application.Handlers.RoomMembers.Commands;
using ChatApp.Application.Handlers.Rooms.DTOs;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.Handlers.Rooms.Orchestrators
{
    public class CreatePrivateRoomOrchestrator : IRequest<CustomeResponse<DTO_CreatePrivateRoomCommand>>
    {
        [JsonIgnore]
        public string? CurrentUserId { get; set; }
        public string? UserId { get; set; }
    }


    public class CreatePrivateRoomOrchestratorHandler : IRequestHandler<CreatePrivateRoomOrchestrator, CustomeResponse<DTO_CreatePrivateRoomCommand>>
    {
        private readonly IGenericRepository<Room> _roomRepository;
        private readonly IMediator _mediator;

        public CreatePrivateRoomOrchestratorHandler(
            IGenericRepository<Room> roomRepository,
            IMediator mediator)
        {
            _roomRepository = roomRepository;
            _mediator = mediator;
        }

        public async Task<CustomeResponse<DTO_CreatePrivateRoomCommand>> Handle(CreatePrivateRoomOrchestrator request, CancellationToken cancellationToken)
        {
            // Step 1: Check if private room already exists between these two users
            var existingRoom = _roomRepository
                .FilterAll(r => r.RoomType == RoomType.Private, include: new[] { "RoomMembers" })
                .FirstOrDefault(r =>
                    r.RoomMembers.Any(m => m.UserId == request.CurrentUserId) &&
                    r.RoomMembers.Any(m => m.UserId == request.UserId)
                );

            if (existingRoom != null)
                return CustomeResponse<DTO_CreatePrivateRoomCommand>.Fail("Private room already exists between these users");

            // Step 2: Create new private room
            var room = new Room
            {
                RoomType = RoomType.Private,
                CreatedByUserId = request.CurrentUserId,
            };

            await _roomRepository.AddAsync(room);
            await _roomRepository.SaveChangesAsync();

            // Step 3: Add both users as members using AddRoomMemberCommand
            var addMembersCommand = new AddRoomMemberCommand
            {
                RoomId = room.Id,
                UserIds = new List<string> { request.CurrentUserId, request.UserId }
            };

            var result = await _mediator.Send(addMembersCommand, cancellationToken);
            if (result.Status != ResponseStatus.Success)
                return CustomeResponse<DTO_CreatePrivateRoomCommand>.Fail("Room created but failed to add members");

            return CustomeResponse<DTO_CreatePrivateRoomCommand>.Success(new DTO_CreatePrivateRoomCommand { RoomId = room.Id, LastUpdated = room.CreatedAt}, "Private room created successfully");
        }
    }
}
    

