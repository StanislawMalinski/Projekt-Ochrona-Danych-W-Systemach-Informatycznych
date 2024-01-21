using Microsoft.AspNetCore.Components.Web;

namespace projekt.Models.Requests
{

    public class TransferRequest
    {
        public TransferRequest()
        {
            AccountNumber = "";
            RecipientAccountNumber = "";
            Recipient = "";
            Value = 0;
            Title = "";
        }
        public required string AccountNumber { get; set; }
        public required string RecipientAccountNumber { get; set; }
        public required string Recipient { get; set; }
        public required decimal Value { get; set; }
        public required string Title { get; set; }
    }
}