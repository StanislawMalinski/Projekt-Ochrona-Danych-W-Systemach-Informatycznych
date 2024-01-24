using projekt.Serivces;
using projekt.Models.Dtos;
namespace projekt.Models.Requests
{
    public class PasswordChangeRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
        public bool IsValid()
        {
            return Validator.validEmail(Email) && Validator.validCode(Code);
        }
    }
}