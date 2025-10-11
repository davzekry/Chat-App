using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.Common.Models;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.Handlers.RoomMembers.Commands
{
    public class AddRoomMemberCommand : IRequest<CustomeResponse<bool>>
    {
        public string RoomId { get; set; }
        public List<string> UserIds { get; set; } = new();
    }

    
    public class AddRoomMemberCommandHandler : IRequestHandler<AddRoomMemberCommand, CustomeResponse<bool>>
    {
        private readonly IGenericRepository<RoomMember> _roomMemberRepository;

        public AddRoomMemberCommandHandler(IGenericRepository<RoomMember> roomMemberRepository)
        {
            _roomMemberRepository = roomMemberRepository;
        }

        public async Task<CustomeResponse<bool>> Handle(AddRoomMemberCommand request, CancellationToken cancellationToken)
        {
            if (request.UserIds == null || !request.UserIds.Any())
                return CustomeResponse<bool>.Fail("No users provided to add to the room");

            var existingMembers = _roomMemberRepository
                .FilterAll(rm => rm.RoomId == request.RoomId)
                .Select(rm => rm.UserId)
                .ToList();

            var newUserIds = request.UserIds
                .Where(uid => !existingMembers.Contains(uid))
                .ToList();

            if (!newUserIds.Any())
                return CustomeResponse<bool>.Fail("All users are already members of this room");

            foreach (var userId in newUserIds)
            {
                var member = new RoomMember
                {
                    RoomId = request.RoomId,
                    UserId = userId,
                    JoinedAt = DateTime.UtcNow
                };

                await _roomMemberRepository.AddAsync(member);
            }

            await _roomMemberRepository.SaveChangesAsync();

            return CustomeResponse<bool>.Success(true, "Members added successfully");
        }
    }
}
