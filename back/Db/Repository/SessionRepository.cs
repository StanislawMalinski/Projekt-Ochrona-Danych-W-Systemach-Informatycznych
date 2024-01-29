using projekt.Db.BankContext;
using projekt.Db.Repository.Interfaces;
using projekt.Models.Dtos;

namespace projekt.Db.Repository;

public class SessionRepository : ISessionRepository
{
    private readonly BankDbContext _bankDbContext;
    public SessionRepository(BankDbContext bankDbContext)
    {
        _bankDbContext = bankDbContext;
    }

    public int CreateSession(int userId, DateTime expirationDate)
    {
        CleanUp();
        Session session = new Session
        {
            SessionId = GenerateSessionId(),
            StartTime = DateTime.Now,
            EndTime = expirationDate,
            UserId = userId
        };
        _bankDbContext.Sessions.Add(session);
        _bankDbContext.SaveChanges();
        return session.SessionId;
    }

    public void DeleteSession(int sessionId)
    {
        CleanUp();
        var session = _bankDbContext.Sessions
            .Where(s => s.SessionId == sessionId)
            .FirstOrDefault();
        if (session == null) return;
        _bankDbContext.Sessions.Remove(session);
        _bankDbContext.SaveChanges();
    }

    public int GetUserIdForSession(int sessionId)
    {
        CleanUp();
        var session = _bankDbContext.Sessions
            .Where(s => s.SessionId == sessionId)
            .FirstOrDefault();
        return session == null ? -1 : session.UserId;
    }

    public bool IsSessionValid(int sessionId, int userId)
    {
        CleanUp();
        var session = _bankDbContext.Sessions
            .Where(s => s.SessionId == sessionId && s.UserId == userId)
            .FirstOrDefault();
        if (session == null) return false;
        if (session.EndTime < DateTime.Now) return false;
        return true;
    }

    private void CleanUp()
    {
        _bankDbContext.Sessions
            .Where(s => s.EndTime < DateTime.Now)
            .ToList()
            .ForEach(s => _bankDbContext.Sessions.Remove(s));
        _bankDbContext.SaveChanges();
    }

    private int GenerateSessionId()
    {
        var maxId = _bankDbContext.Sessions
            .Max(s => s.SessionId);
        return maxId <= 0 ? (maxId + 1) : 1;
    }
}