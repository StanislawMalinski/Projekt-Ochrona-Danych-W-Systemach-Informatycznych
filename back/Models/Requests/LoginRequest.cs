using  projekt.Serivces;
using projekt.Models.Dtos;
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
        public required Token token { get; set; }
        public bool IsValid()
        {
            return Validator.validEmail(Email);
        }
    }
}