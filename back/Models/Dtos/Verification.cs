namespace projekt.Models.Dtos
{
    public class Verification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Code { get; set; }
        public DateTime Date {get; set;}
    }
}