using projekt.Models.Dtos;


namespace projekt.Db.Repository.Interfaces;

public interface ISessionRepository
{
    public int CreateSession(int userId);
    public void DeleteSession(int sessionId);
    public bool IsSessionValid(int sessionId, int userId);
}
