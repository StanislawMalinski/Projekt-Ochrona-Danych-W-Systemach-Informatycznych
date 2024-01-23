using projekt.Serivces;

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

        public bool IsValid()
        {
            return  Validator.validNumber(AccountNumber) && 
                    Validator.validNumber(RecipientAccountNumber) && 
                    Validator.validName(Recipient) && 
                    Validator.validText(Title) && Value > 0;
        }
    }
}