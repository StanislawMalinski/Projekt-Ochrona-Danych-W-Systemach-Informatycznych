
namespace projekt.Models.Responses
{
    public class LoginResponse
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
        public required string Message { get; set; }
        public required bool Success { get; set; }
    }
}