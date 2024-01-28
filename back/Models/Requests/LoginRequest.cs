using  projekt.Services;
using projekt.Models.Dtos;
namespace projekt.Models.Requests
{
    public class LoginRequest :BasicRequest
    {
        public LoginRequest()
        {
            Email = "";
            Password = "";
        }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public override string IsValid()
        {
            if (!Validator.validEmail(Email) ) 
                return "Email is not valid";
            return "";
        }
    }
}