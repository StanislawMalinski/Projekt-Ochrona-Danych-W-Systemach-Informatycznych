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
        }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public bool IsValid()
        {
            return Validator.validEmail(Email) && Validator.validName(Name);
        }
    }
}