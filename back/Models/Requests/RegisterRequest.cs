using projekt.Serivces;
using projekt.Models.Dtos;
namespace projekt.Models.Requests
{

    public class RegisterRequest
    {
        public RegisterRequest()
        {
            Email = "";
            Password = "";
            Name ="";
            token = new Token();
        }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public required Token token { get; set; }
        public bool IsValid()
        {
            return Validator.validEmail(Email) && Validator.validName(Name);
        }
    }
}