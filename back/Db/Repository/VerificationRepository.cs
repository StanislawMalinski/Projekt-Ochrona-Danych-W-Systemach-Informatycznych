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

        public Verification? GetVerification(string email)
        {
            cleanup();
            return _dbcontext.Verifications
                .Where(v => v.Email == email)
                .OrderByDescending(v => v.Date)
                .FirstOrDefault();
        }

        public bool CheckIfVerificationIsValid(string email, string code)
        {
            code = _cryptoService.HashPassword(code, email);
            cleanup();
            return _dbcontext.Verifications
                .Where(v => v.Email == email && v.Code == code)
                .OrderByDescending(v => v.Date)
                .FirstOrDefault()
                != null;
        }

        public void CreateVerification(string email, string code)
        {
            var verification = GetVerification(email);
            if (verification != null) DeleteVerification(email);
            verification = new Verification
            {
                Email = email,
                Code = _cryptoService.HashPassword(code, email),
                Date = System.DateTime.Now
            };
            _dbcontext.Verifications.Add(verification);
            cleanup();
            _dbcontext.SaveChanges();
        }

        public bool DeleteVerification(string email)
        {
            var verification = GetVerification(email);
            if (verification == null) return false;
            _dbcontext.Verifications.Remove(verification);
            _dbcontext.SaveChanges();
            cleanup();
            return true;
        }

        private void cleanup(){
            var verifications = _dbcontext.Verifications
                .Where(v => v.Date.AddMinutes(5) < System.DateTime.Now);
            _dbcontext.Verifications.RemoveRange(verifications);
            _dbcontext.SaveChanges();
        }
    }
}