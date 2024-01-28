
namespace projekt.Models.Responses
{
    public class LoginResponse : SimpleResponse
    {
        public LoginResponse()
        {
            AccountNumber = "";
            Balance = 0;
            Message = "";
            Success = false;
        }
        public required string AccountNumber { get; set; }
        public required decimal Balance { get; set; }
    }
}