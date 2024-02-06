using projekt.Models.Dtos;

namespace projekt.Db.Repository
{
    public interface ITransferRepository
    {
        public List<Transfer> GetHistory(string accountNumber);
        public void NewTransfer(Transfer transfer);
    }
}