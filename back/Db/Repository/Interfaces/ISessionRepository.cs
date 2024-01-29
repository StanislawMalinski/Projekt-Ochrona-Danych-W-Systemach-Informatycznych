using projekt.Models.Dtos;


namespace projekt.Db.Repository.Interfaces;

public interface ISessionRepository
{
    public int CreateSession(int userId, DateTime expirationDate);
    public void DeleteSession(int sessionId);
    public int GetUserIdForSession(int sessionId);
    public bool IsSessionValid(int sessionId, int userId);
}
