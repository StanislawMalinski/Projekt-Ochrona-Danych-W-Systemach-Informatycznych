using System.ComponentModel.DataAnnotations.Schema;

namespace projekt.Models.Dtos;

public class Session
{
    public int SessionId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    [ForeignKey("Account")]
    public int UserId { get; set; }
}
