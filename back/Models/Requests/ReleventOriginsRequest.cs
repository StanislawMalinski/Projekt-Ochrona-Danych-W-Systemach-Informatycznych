using projekt.Models.Dtos;

namespace projekt.Models.Requests
{
    public class ReleventOriginsRequest : BasicRequest
    {
        public Token Token { get; set; }

        public override string IsValid()
        {
            return "";
        }
    }
}