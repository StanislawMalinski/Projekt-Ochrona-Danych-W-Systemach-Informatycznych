
namespace projekt.Models.Dtos;

public class Account{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? AccountNumber {get; set;}
    public string? Email {get; set;}
    public decimal Balance {get; set;}
    public string? Password {get; set;}
    public string? Salt {get; set;}
    public bool IsVerified {get; set;}

}