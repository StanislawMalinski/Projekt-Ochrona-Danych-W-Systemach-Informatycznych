namespace projekt.Models.Requests
{
    public class LoginRequest
    {
        public LoginRequest()
        {
            Email = "";
            Password = "";
        }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}