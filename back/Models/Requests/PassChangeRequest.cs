using System.ComponentModel.DataAnnotations;

namespace projekt.Models.Requests
{
    public class PassChangeRequest
    {
        public PassChangeRequest()
        {
            Email = "";
        } 
        public required string Email { get; set; }
    }
}