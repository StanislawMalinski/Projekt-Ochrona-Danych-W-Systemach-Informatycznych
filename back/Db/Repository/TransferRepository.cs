
using projekt.Db.BankContext;
using projekt.Models.Dtos;
namespace projekt.Db.Repository;

public class TransferRepository : ITransferRepository{
    private readonly BankDbContext _bankDbContext;

    public TransferRepository(BankDbContext bankDbContext)
    {
        _bankDbContext = bankDbContext;
    }

    public void NewTransfer(Transfer transfer){
        _bankDbContext.Transfers.Add(transfer);
        _bankDbContext.SaveChanges();
    }


    public List<Transfer> GetHistory(string accountNumber){
        var toMe = _bankDbContext.Transfers.Where(t => t.AccountNumber == accountNumber).ToList();
        var fromMe = _bankDbContext.Transfers.Where(t => t.RecipentAccountNumber== accountNumber).ToList();
        var history = toMe.Concat(fromMe).ToList();
        history.Sort((x,y) => DateTime.Compare(x.Date, y.Date));
        return history;
    }
}