namespace projekt.Models.Dtos
{
    public class Verification
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime Date {get; set;}
    }
}