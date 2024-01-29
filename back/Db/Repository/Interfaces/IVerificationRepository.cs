using projekt.Models.Dtos;

namespace projekt.Services;
public interface IVerificationRepository
{
    public void CreateVerification(string email, string code);
    public bool CheckIfVerificationIsValid(string email, string code);
    public bool DeleteVerification(string email);
    public Verification? GetVerification(string email);
}