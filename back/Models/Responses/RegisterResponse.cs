
using projekt.Models.Dtos;
namespace projekt.Models.Responses
{
    public class RegisterResponse
    {
        public RegisterResponse()
        {
            AccountNumber = "";
            Balance = 0;
            History = new List<Transfer>();
            Message = "";
            Success = false;
        }
        public required string AccountNumber { get; set; }
        public required decimal Balance { get; set; }
        public required List<Transfer> History { get; set; }
        public required string Message { get; set; }
        public required bool Success { get; set; }
    }
}