using BackendBlogServicesApi.DTOs;

namespace BackendBlogServicesApi.Entity
{

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = null;
        public bool Estado { get; set; } = true;
    }
    public class UserLoginResponseDto
    {
        public TokenInfo TokenDecode { get; set; }
        public UserDto User { get; set; }
    }

    public class TokenInfo
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string ExpirationString => GetExpirationString();
        private string GetExpirationString()
        {
            var remainingTime = Expiration - DateTime.UtcNow;

            if (remainingTime.TotalSeconds <= 0)
            {
                return "Expired";
            }

            return $"{remainingTime.Hours} horas, {remainingTime.Minutes} minutos, {remainingTime.Seconds} segundos";
        }
    }





}
