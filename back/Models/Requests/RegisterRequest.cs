using projekt.Services;
using projekt.Models.Dtos;
namespace projekt.Models.Requests
{

    public class RegisterRequest : BasicRequest
    {
        public RegisterRequest()
        {
            Email = "";
            Password = "";
            Name ="";
        }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public override string IsValid()
        {
            if (!Validator.validEmail(Email) ) 
                return "Email is not valid";
            if (!Validator.validName(Name)) return "Name is not valid";
            return "";
        }
    }
}