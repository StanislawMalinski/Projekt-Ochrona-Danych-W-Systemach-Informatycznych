using projekt.Services;
using projekt.Models.Dtos;
namespace projekt.Models.Requests
{
    public class PasswordChangeRequest : BasicRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
        public override string IsValid()
        {
            if (!Validator.validCode(Code)) return "Code is not valid";
            if (string.IsNullOrEmpty(Email))
                return "Email is not valid";
            return "";
        }
    }
}