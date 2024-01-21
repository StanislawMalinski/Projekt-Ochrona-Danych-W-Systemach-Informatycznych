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
            return _dbcontext.Verifications
                .Where(v => v.Email == email)
                .OrderByDescending(v => v.Date)
                .FirstOrDefault();
        }
        public void CreateVerification(string email, string code)
        {
            var verification = new Verification
            {
                Email = email,
                Code = code,
                Date = System.DateTime.Now
            };
            _dbcontext.Verifications.Add(verification);
            _dbcontext.SaveChanges();
        }
    }
}