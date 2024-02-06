using projekt.Db.BankContext;
using projekt.Models.Dtos;
using projekt.Services.Interfaces;
using projekt.Services;

namespace projekt.Db.Repository
{
    public class VerificationRepository : IVerificationRepository
    {
        private readonly BankDbContext _dbcontext;
        private readonly ICryptoService _cryptoService;
        public VerificationRepository(BankDbContext context, 
            ICryptoService cryptoService)
        {
            _dbcontext = context;
            _cryptoService = cryptoService;
        }

        public Verification? GetVerification(int userId)
        {
            cleanup();
            return _dbcontext.Verifications
                .Where(v => v.UserId == userId)
                .FirstOrDefault();
        }

        public bool CheckIfVerificationIsValid(int userId, string code)
        {
            cleanup();
            code = _cryptoService.HashPassword(code, "" + userId);
            return _dbcontext.Verifications
                .Where(v => v.UserId == userId && v.Code == code)
                .FirstOrDefault()
                != null;
        }

        public void CreateVerification(int userId, string code)
        {
            var verification = GetVerification(userId);
            if (verification != null) DeleteVerification(userId);
            code = _cryptoService.HashPassword(code, "" + userId);
            verification = new Verification
            {
                UserId = userId,
                Code = code,
                Date = DateTime.Now
            };
            _dbcontext.Verifications.Add(verification);
            cleanup();
            _dbcontext.SaveChanges();
        }

        public bool DeleteVerification(int userId)
        {
            var verification = GetVerification(userId);
            if (verification == null) return false;
            _dbcontext.Verifications.Remove(verification);
            _dbcontext.SaveChanges();
            cleanup();
            return true;
        }

        private void cleanup(){
            var verifications = _dbcontext.Verifications
                .Where(v => v.Date.AddMinutes(5) < DateTime.Now);
            _dbcontext.Verifications.RemoveRange(verifications);
            _dbcontext.SaveChanges();
        }
    }
}