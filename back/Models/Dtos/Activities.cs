using projekt.Models.Enums;

namespace projekt.Models.Dtos;

public class Activity
{
    public int Id {get; set;}
    public string IP {get; set;}
    public DateTime Date {get; set;}
    public ActivityType Type {get; set;}   
    public bool Success {get; set;}
}