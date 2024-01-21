
namespace projekt.Models.Responses
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            AccountNumber = "";
            Balance = 0;
            Message = "";

        }
        public required string AccountNumber { get; set; }
        public required decimal Balance { get; set; }
        public required string Message { get; set; }
    }
}