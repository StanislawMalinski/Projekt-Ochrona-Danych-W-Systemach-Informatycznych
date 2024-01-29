using projekt.Models.Dtos;

namespace projekt.Services;
public interface IVerificationRepository
{
    public void CreateVerification(int userId, string code);
    public bool CheckIfVerificationIsValid(int userId, string code);
    public bool DeleteVerification(int userId);
    public Verification? GetVerification(int userId);
}