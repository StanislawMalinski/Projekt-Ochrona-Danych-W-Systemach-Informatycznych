using projekt.Models.Dtos;

namespace projekt.Services.Interfaces;
public interface IAccessService
{
    bool ShouldReplayToOrigin(string origin);
    int GetUserId(Token token);
    Token GetToken(int usedId);
    bool VerifyToken(Token token);
    void RemoveSession(Token token);
}
