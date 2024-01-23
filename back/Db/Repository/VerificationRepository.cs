using projekt.Db.BankContext;
using projekt.Models.Dtos;
using projekt.Serivces;

namespace projekt.Db.Repository
{
    public class VerificationRepository : IVerificationRepository
    {
        private readonly BankDbContext _dbcontext;
        public VerificationRepository(BankDbContext context)
        {
            _dbcontext = context;
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
            code = CryptoService.HashPassword(code, email);
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
                Code = CryptoService.HashPassword(code, email),
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