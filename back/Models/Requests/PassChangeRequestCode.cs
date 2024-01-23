using projekt.Serivces;

namespace projekt.Models.Requests
{
    public class PassChangeRequestCode
    {
        public PassChangeRequestCode()
        {
            Email = "";
        } 
        public required string Email { get; set; }
        public bool IsValid()
        {
            return Validator.validEmail(Email);
        }
    }
}