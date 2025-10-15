using Chat_Application.Hubs;
using ChatApp.Application.Common.Models;
using ChatApp.Application.Handlers.Messages.Validators;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Helper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Application.Handlers.Messages.Commands
{
    public class SendMessageCommand : IRequest<CustomeResponse<bool>>
    {
        [BindNever]
        public string? UserId { get; set; }
        public string? RoomId { get; set; }
        public string? MessageText { get; set; }
        public MessageType MessageType { get; set; } = MessageType.Text;

        // For file or audio uploads
        public IFormFile? File { get; set; }
        public IFormFile? AudioFile { get; set; }
    }

    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, CustomeResponse<bool>>
    {
        private readonly IGenericRepository<Message> _messageRepository;
        private readonly IGenericRepository<FileMessage> _fileMessageRepository;
        private readonly IGenericRepository<VoiceMessage> _voiceMessageRepository;
        private readonly IHubContext<Chathub> _hubContext;

        public SendMessageCommandHandler(
            IGenericRepository<Message> messageRepository,
            IGenericRepository<FileMessage> fileMessageRepository,
            IGenericRepository<VoiceMessage> voiceMessageRepository,
            IHubContext<Chathub> hubContext)
        {
            _messageRepository = messageRepository;
            _fileMessageRepository = fileMessageRepository;
            _voiceMessageRepository = voiceMessageRepository;
            _hubContext = hubContext;
        }

        public async Task<CustomeResponse<bool>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var validationRules = new SendMessageCommandValidator();
            ValidationResult validationResult = await validationRules.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                List<string> errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                return CustomeResponse<bool>.Fail($"Validation failed in 'SendMessageCommand'.\n{(string.Join(Environment.NewLine, errors))}");
            }
            // Create base message entity
            var message = new Message
            {
                UserId = request.UserId ?? string.Empty,
                RoomId = request.RoomId ?? string.Empty,
                MessageText = request.MessageText,
                MessageType = request.MessageType,
                CreatedAt = DateTime.UtcNow
            };

            await _messageRepository.AddAsync(message);

            // Initialize placeholders for file/audio URLs
            string? filePath = null;
            string? audioPath = null;

            switch (request.MessageType)
            {
                case MessageType.file:
                    if (request.File == null)
                        return CustomeResponse<bool>.Fail("File is required for file message.");

                    filePath = await FileHelper.UploadFileAsync(request.File);

                    if (filePath.StartsWith("Error") || filePath.Contains("Invalid"))
                        return CustomeResponse<bool>.Fail(filePath);

                    var fileMessage = new FileMessage
                    {
                        MessageId = message.Id,
                        FileName = request.File.FileName,
                        FilePath = filePath,
                        FileSize = request.File.Length
                    };

                    await _fileMessageRepository.AddAsync(fileMessage);
                    break;

                case MessageType.Audio:
                    if (request.AudioFile == null)
                        return CustomeResponse<bool>.Fail("Audio file is required for audio message.");

                    audioPath = await FileHelper.UploadFileAsync(request.AudioFile);

                    if (audioPath.StartsWith("Error") || audioPath.Contains("Invalid"))
                        return CustomeResponse<bool>.Fail(audioPath);

                    var voiceMessage = new VoiceMessage
                    {
                        MessageId = message.Id,
                        UserId = request.UserId,
                        AudioFilePath = audioPath,
                        DurationSeconds = 0 // can be set from client if known
                    };

                    await _voiceMessageRepository.AddAsync(voiceMessage);
                    break;

                case MessageType.Text:
                default:
                    // Nothing extra for text messages
                    break;
            }

            await _messageRepository.SaveChangesAsync();

            // ✅ Broadcast message to all users in the same room
            var broadcastPayload = new
            {
                MessageId = message.Id,
                RoomId = message.RoomId,
                UserId = message.UserId,
                MessageText = message.MessageText,
                MessageType = message.MessageType.ToString(),
                FilePath = filePath,
                AudioFilePath = audioPath,
                CreatedAt = message.CreatedAt,
                UserName = message.User.UserName,
                UserProfileImage = message.User.ImagePath
            };

            // Change this line in SendMessageCommandHandler.cs
            await _hubContext.Clients.Group(request.RoomId.ToString()).SendAsync("ReceiveMessage", broadcastPayload, cancellationToken);


            return CustomeResponse<bool>.Success(true, "Message sent successfully.");
        }
    }
}
