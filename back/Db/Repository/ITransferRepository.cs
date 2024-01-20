using projekt.Models.Dtos;

namespace projekt.Db.Repository
{
    public interface ITransferRepository
    {
        public Transfer NewTransfer(Transfer transfer);
        public List<Transfer> GetHistory(string accountNumber);
    }
}