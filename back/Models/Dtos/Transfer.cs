
namespace projekt.Models.Dtos;

public class Transfer{
    public int Id {get; set;}
    public string AccountNumber {get; set;}
    public string RecipentAccountNumber {get; set;}
    public string Recipant {get; set;}
    public string Title {get; set;}
    public decimal Value {get; set;}
}