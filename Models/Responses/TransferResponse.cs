

namespace projekt.Models.Responses
{
    public record TransferResponse
    {
        public TransferResponse()
        {
            AccountNumber = "";
            Balance = 0;
            Success = false;
            Message = "";
        }
        public required string AccountNumber { get; set; }
        public required decimal Balance { get; set; }
        public required bool Success { get; set; }
        public required string Message { get; set; }
    }
}