namespace projekt.Models.Requests
{

    public record TransferRequest
    {
        public TransferRequest()
        {
            AccountNumber = "";
            RecipientAccountNumber = "";
            Amount = 0;
        }
        public required string AccountNumber { get; set; }
        public required string RecipientAccountNumber { get; set; }
        public required decimal Amount { get; set; }
    }
}