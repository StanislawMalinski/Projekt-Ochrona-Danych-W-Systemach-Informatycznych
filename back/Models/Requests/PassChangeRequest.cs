using System.ComponentModel.DataAnnotations;

namespace projekt.Models.Requests
{
    public record PassChangeRequest
    {
        public PassChangeRequest()
        {
            Email = "";
        } 
        public required string Email { get; set; }
    }
}