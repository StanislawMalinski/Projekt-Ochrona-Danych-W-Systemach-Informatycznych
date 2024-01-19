using projekt.Models.Dtos;

namespace projekt.Db.Repository
{
    public interface ITransferRepository
    {
        Transfer NewTransfer(Transfer transfer);
    }
}