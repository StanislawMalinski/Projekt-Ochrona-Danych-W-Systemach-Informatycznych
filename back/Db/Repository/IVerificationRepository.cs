using projekt.Models.Dtos;

namespace projekt.Serivces;
public interface IVerificationRepository
{
    public Verification? GetVerification(string email);
    public void CreateVerification(string email, string code);
}