using Microsoft.AspNetCore.SignalR;

namespace Chat_Application.Hubs
{
    public class Chathub : Hub
    {
        // Called from Angular when a user opens a chat room
        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            await Clients.Group(roomId)
                .SendAsync("UserJoined", $"{Context.ConnectionId} joined room {roomId}");
        }

        // Called from Angular when a user leaves a chat room
        public async Task LeaveRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

            await Clients.Group(roomId)
                .SendAsync("UserLeft", $"{Context.ConnectionId} left room {roomId}");
        }

        // Used internally by your handler to send a message to everyone in the group
        public async Task SendMessageToRoom(object message)
        {
            string roomId = message?.GetType()
                                   ?.GetProperty("RoomId")
                                   ?.GetValue(message)?
                                   .ToString();

            if (string.IsNullOrEmpty(roomId))
                return;

            await Clients.Group(roomId).SendAsync("ReceiveMessage", message);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // You can optionally clean up user group memberships here
            await base.OnDisconnectedAsync(exception);
        }
    }
}
