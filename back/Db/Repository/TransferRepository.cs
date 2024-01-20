
using projekt.Db.BankContext;
using projekt.Models.Dtos;
namespace projekt.Db.Repository;

public class TransferRepository : ITransferRepository{
    private readonly BankDbContext _bankDbContext;

    public TransferRepository(BankDbContext bankDbContext)
    {
        _bankDbContext = bankDbContext;
    }

    public Transfer NewTransfer(Transfer transfer){
        throw new NotImplementedException();
    }


    public List<Transfer> GetHistory(string accountNumber){
        var toMe = _bankDbContext.Transfers.Where(t => t.AccountNumber == accountNumber).ToList();
        var fromMe = _bankDbContext.Transfers.Where(t => t.RecipentAccountNumber== accountNumber).ToList();
        var history = toMe.Concat(fromMe).ToList();
        return history;
    }
}