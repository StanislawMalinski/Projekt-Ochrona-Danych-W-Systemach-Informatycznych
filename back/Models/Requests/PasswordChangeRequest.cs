

namespace projekt.Models.Requests
{
    public class PasswordChangeRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
    }
}