
namespace ChatApp.Application.Handles.Authentication.DTOs
{
    public class LoginJwtResponseDTO
    {
        public DateTime ExpireDate { get; set; }
        public string Token { get; set; }
    }
}
