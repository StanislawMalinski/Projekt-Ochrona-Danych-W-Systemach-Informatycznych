namespace projekt.Models.Requests
{

    public record RegisterRequest
    {
        public RegisterRequest()
        {
            Email = "";
            Password = "";
        }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}