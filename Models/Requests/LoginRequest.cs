namespace projekt.Models.Requests
{
    public record LoginRequest
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