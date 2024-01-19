
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

}