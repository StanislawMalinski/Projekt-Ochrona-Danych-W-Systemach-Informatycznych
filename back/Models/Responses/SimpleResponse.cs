

namespace projekt.Models.Responses
{
    public class SimpleResponse
    {
        public SimpleResponse()
        {
            Success = false;
            Message = "";
        }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}