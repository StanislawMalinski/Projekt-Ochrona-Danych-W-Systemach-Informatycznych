using projekt.Services;

using projekt.Models.Dtos;
namespace projekt.Models.Requests
{
    public class PassChangeRequestCode: BasicRequest
    {
        public PassChangeRequestCode()
        {
            Email = "";
        } 
        public required string Email { get; set; }
        public override string IsValid()
        {
            if (!Validator.validEmail(Email)) return "Email is not valid";
            return "";
        }
    }
}