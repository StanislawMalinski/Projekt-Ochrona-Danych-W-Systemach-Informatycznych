

namespace projekt.Models.Responses
{
    public class BasicResponse
    {
        public BasicResponse()
        {
            Success = false;
            Message = "";
        }
        public BasicResponse(string message)
        {
            Success =false;
            Message = message;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}