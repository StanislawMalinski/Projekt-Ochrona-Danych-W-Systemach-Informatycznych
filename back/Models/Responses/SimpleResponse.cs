

namespace projekt.Models.Responses
{
    public class BasicResponse
    {
        public BasicResponse()
        {
            Success = false;
            Message = "";
        }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}