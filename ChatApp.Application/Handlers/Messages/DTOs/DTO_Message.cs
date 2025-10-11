using System;
using ChatApp.Application.Handlers.Messages.DTOs;
using ChatApp.Domain.Enums;

namespace ChatApp.Application.Messages.DTOs
{
    public class DTO_Message
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfileImage { get; set; }
        public string? MessageText { get; set; }
        public MessageType MessageType { get; set; }
        public bool IsEdited { get; set; }
        public DateTime CreatedAt { get; set; }
        public DTO_FileMessage? FileMessage { get; set; }
        public DTO_VoiceMessage? VoiceMessage { get; set; }


        // Derived (optional): to simplify front-end rendering
        public string DisplayContent => MessageType switch
        {
            MessageType.Text => MessageText ?? string.Empty,
            MessageType.file => FileMessage?.FilePath ?? string.Empty,
            MessageType.Audio => VoiceMessage?.AudioUrl ?? string.Empty,
            _ => MessageText ?? string.Empty
        };
    }
}
