using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.Common.Models;
using ChatApp.Application.Handlers.Messages.DTOs;
using ChatApp.Application.Messages.DTOs;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Handlers.Messages.Queries
{
    public class GetAllMessagesByRoomIdQuery : IRequest<CustomeResponse<DTO_GetAllMessagesByRoomIdQuery>>
    {
        public string RoomId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public bool OrderDescending { get; set; } = true; // Latest messages first
    }

    public class GetAllMessagesByRoomIdQueryHandler : IRequestHandler<GetAllMessagesByRoomIdQuery, CustomeResponse<DTO_GetAllMessagesByRoomIdQuery>>
    {
        private readonly IGenericRepository<Message> _messageRepo;
        private readonly IGenericRepository<Room> _roomRepo;

        public GetAllMessagesByRoomIdQueryHandler(IGenericRepository<Message> messageRepo, IGenericRepository<Room> roomRepo)
        {
            this._messageRepo = messageRepo;
            this._roomRepo = roomRepo;
        }

        public async Task<CustomeResponse<DTO_GetAllMessagesByRoomIdQuery>> Handle(GetAllMessagesByRoomIdQuery request, CancellationToken cancellationToken)
        {
            var roomExists = await _roomRepo.GetByIdAsync(request.RoomId);
            if (roomExists == null)
                return CustomeResponse<DTO_GetAllMessagesByRoomIdQuery>.Error("Room Not Found 'GetAllMessagesByRoomIdQueryHandler'");
            
            // Filter predicate
            Expression<Func<Message, bool>> filterPredicate = m => m.RoomId == request.RoomId;

            // Order expression
            //Expression<Func<Message, object>> orderExpression = m => m.CreatedDate;

            // Include related entities
            Func<IQueryable<Message>, IQueryable<Message>> includeFunc = query =>
                query.Include(m => m.User)
                     .Include(m => m.FileMessage)
                     .Include(m => m.VoiceMessage);

            // Get paginated results
            var pageResult = await _messageRepo.GetPaginatedAsync(
                                                    pageNumber: request.PageNumber,
                                                    pageSize: request.PageSize,
                                                    filter: filterPredicate,
                                                    null,
                                                    include: includeFunc);

            // Cast to List for further processing
            var messages = pageResult.Items.ToList();

            // Map to DTOs manually
            var messageDtos = messages.Select(message => new DTO_Message
            {
                Id = message.Id,
                UserId = message.UserId,
                UserName = message.User?.UserName ?? "Unknown User",
                UserProfileImage = message.User?.ImagePath ?? "",
                MessageText = message.MessageText,
                MessageType = message.MessageType,
                IsEdited = message.IsEdited,
                CreatedAt = message.CreatedAt,
                FileMessage = (message.MessageType == MessageType.file)
                    && message.FileMessage != null
                    ? new DTO_FileMessage
                    {
                        Id = message.FileMessage.Id,
                        FileName = message.FileMessage.FileName,
                        FilePath = FileHelper.GetFilePath(message.FileMessage.FilePath),
                        FileSize = message.FileMessage.FileSize
                    }
                    : null,
                // This part need 
                //VoiceMessage = message.MessageType == MessageType.Audio && message.VoiceMessage != null
                //? new DTO_VoiceMessage
                //{
                //    Id = message.VoiceMessage.Id,
                //    AudioUrl = FileHelper.GetFilePath(message.VoiceMessage.AudioFilePath),
                //    DurationSeconds = message.VoiceMessage.DurationSeconds
                //}
                //: null
            }).ToList();

            var hasMore = (request.PageNumber * request.PageSize) < pageResult.TotalCount;


            return CustomeResponse<DTO_GetAllMessagesByRoomIdQuery>.Success( new DTO_GetAllMessagesByRoomIdQuery
            {
                Messages = messageDtos,
                TotalCount = pageResult.TotalCount,
                HasMore = hasMore
            });

            //_logger.LogInformation(
            //    "Retrieved {MessageCount} messages for room {RoomId}. Total: {TotalCount}, Page: {PageNumber}",
            //    messageDtos.Count,
            //    request.RoomId,
            //    pageResult.TotalCount,
            //    request.PageNumber);
        }
    }
}
