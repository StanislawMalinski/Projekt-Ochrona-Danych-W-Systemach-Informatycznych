using System.ComponentModel.DataAnnotations;

namespace projekt.Models.Requests
{
    public class PassChangeRequestCode
    {
        public PassChangeRequestCode()
        {
            Email = "";
        } 
        public required string Email { get; set; }
    }
}