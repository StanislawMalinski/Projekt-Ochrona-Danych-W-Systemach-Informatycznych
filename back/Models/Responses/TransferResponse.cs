using projekt.Models.Dtos;

namespace projekt.Models.Responses
{
    public class TransferResponse
    {
        public TransferResponse()
        {
            AccountNumber = "";
            Balance = 0;
            History = new List<Transfer>();
            Success = false;
            Message = "";
        }
        public required string AccountNumber { get; set; }
        public required decimal Balance { get; set; }
        public required List<Transfer> History { get; set; }
        public required bool Success { get; set; }
        public required string Message { get; set; }
    }
}