using projekt.Services;
using projekt.Models.Dtos;

namespace projekt.Models.Requests
{

    public class TransferRequest : BasicRequest
    {
        public TransferRequest()
        {
            AccountNumber = "";
            RecipientAccountNumber = "";
            Recipient = "";
            Value = 0;
            Title = "";
            Token = new Token();
        }
        public required string AccountNumber { get; set; }
        public required string RecipientAccountNumber { get; set; }
        public required string Recipient { get; set; }
        public required decimal Value { get; set; }
        public required string Title { get; set; }
        public required Token Token { get; set; }
        public override string IsValid()
        {
            if(!Validator.validNumber(AccountNumber)) return "Account number is not valid";
            if(!Validator.validNumber(RecipientAccountNumber)) return "Recipient account number is not valid";
            if(!Validator.validName(Recipient)) return "Recipient name is not valid";
            if(!Validator.validText(Title)) return "Title is not valid";
            if( Value <= 0) return "Value is not valid";
            return "";
        }
    }
}