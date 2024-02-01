

namespace projekt.Models.Responses
{
    public class ReleventOriginsResponse : BasicResponse
    {
        public ReleventOriginsResponse()
        {
        }
        public ReleventOriginsResponse(string message)
        {
            Message = message;
            Success = false;
        }
        public List<string> Origins { get; set; }
    }
}