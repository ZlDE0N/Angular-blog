namespace BackendBlogServicesApi.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = null;
        public bool Estado { get; set; }
    }

    public class UserLoginDto
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }


    public class ValidationResultToken
    {
        public bool IsValid { get; set; }
        public int? UserId { get; set; }
        public string Message { get; set; }
    }

    public class TokenValidationRequest
    {
        public string Token { get; set; }
    }

}
