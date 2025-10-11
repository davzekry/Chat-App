using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.Handlers.Messages.Commands;
using ChatApp.Domain.Enums;
using FluentValidation;

namespace ChatApp.Application.Handlers.Messages.Validators
{
    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.RoomId)
                .NotEmpty().WithMessage("RoomId is required.");

            RuleFor(x => x.MessageType)
                .IsInEnum().WithMessage("Invalid message type.");

            When(x => x.MessageType == MessageType.Text, () =>
            {
                RuleFor(x => x.MessageText)
                    .NotEmpty().WithMessage("MessageText is required for text messages.")
                    .MaximumLength(150);
            });

            When(x => x.MessageType == MessageType.file, () =>
            {
                RuleFor(x => x.File)
                    .NotNull().WithMessage("File is required for file messages.")
                    .Must(f => f.Length > 0).WithMessage("File cannot be empty.")
                    .Must(f => f.Length <= 5 * 1024 * 1024).WithMessage("File size cannot exceed 5MB.");
            });

            When(x => x.MessageType == MessageType.Audio, () =>
            {
                RuleFor(x => x.AudioFile)
                    .NotNull().WithMessage("Audio file is required for audio messages.")
                    .Must(f => f.Length > 0).WithMessage("Audio file cannot be empty.")
                    .Must(f => f.Length <= 5 * 1024 * 1024).WithMessage("Audio file size cannot exceed 5MB.");
            });
        }
    }
}
