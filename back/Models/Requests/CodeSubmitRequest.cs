

using projekt.Services;
using projekt.Models.Dtos;
namespace projekt.Models.Requests;

public class CodeSubmitRequest :BasicRequest
{
    public string Email { get; set; }
    public string Code { get; set; }
    public override string IsValid()
    {
        if (!Validator.validEmail(Email)) return "Email is not valid";
        if (!Validator.validCode(Code)) return "Code is not valid";
        return "";
    }
}