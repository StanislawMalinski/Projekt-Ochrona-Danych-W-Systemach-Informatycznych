namespace projekt.Models.Dtos;

public class Session
{
    public int SessionId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int UserId { get; set; }
}
