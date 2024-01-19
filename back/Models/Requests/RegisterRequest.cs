namespace projekt.Models.Requests
{

    public record RegisterRequest
    {
        public RegisterRequest()
        {
            AccountNumber = "";
            RecipientAccountNumber = "";
            Amount = 0;
            Password = "";
        }
        public required string AccountNumber { get; set; }
        public required string RecipientAccountNumber { get; set; }
        public required decimal Amount { get; set; }
        public required string Password { get; set; }
    }
}