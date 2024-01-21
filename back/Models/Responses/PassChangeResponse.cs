

namespace projekt.Models.Responses
{
    public class PassChangeResponse
    {
        public PassChangeResponse()
        {
            Message = "";
            Success = false;
        }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}