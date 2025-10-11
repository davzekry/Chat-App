using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.Common.Models;
using ChatApp.Application.Handlers.Rooms.DTOs;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Handlers.Rooms.Queries
{
    public class GetAllRoomsByUserIdQuery : IRequest<CustomeResponse<List<DTO_GetAllRoomsByUserIdQuery>>>
    {
        public string? UserId { get; set; }
    }

    public class GetAllRoomsByUserIdQueryHandler
        : IRequestHandler<GetAllRoomsByUserIdQuery, CustomeResponse<List<DTO_GetAllRoomsByUserIdQuery>>>
    {
        private readonly IGenericRepository<Room> _roomRepository;

        public GetAllRoomsByUserIdQueryHandler(IGenericRepository<Room> roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<CustomeResponse<List<DTO_GetAllRoomsByUserIdQuery>>> Handle(
            GetAllRoomsByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserId))
                return CustomeResponse<List<DTO_GetAllRoomsByUserIdQuery>>.Fail("UserId is required.");

            // Include RoomMembers and Messages to get both membership and last message info
            var roomsQuery = _roomRepository
                .GetAll(new[] { "RoomMembers", "Messages" })
                .Where(r => !r.IsDeleted &&
                            (r.CreatedByUserId == request.UserId ||
                             r.RoomMembers.Any(m => m.UserId == request.UserId)));

            // Project only needed fields
            var rooms = await roomsQuery
                .Select(r => new DTO_GetAllRoomsByUserIdQuery
                {
                    RoomId = r.Id,
                    RoomName = r.RoomName,
                    ImagePath = r.ImagePath,
                    RoomType = r.RoomType,
                    LastMessageAt = r.Messages
                        .OrderByDescending(m => m.CreatedAt)
                        .Select(m => m.CreatedAt)
                        .FirstOrDefault()
                })
                .OrderByDescending(r => r.LastMessageAt)
                .ToListAsync(cancellationToken);

            return CustomeResponse<List<DTO_GetAllRoomsByUserIdQuery>>.Success(rooms, "Rooms retrieved successfully");
        }
    }
}

