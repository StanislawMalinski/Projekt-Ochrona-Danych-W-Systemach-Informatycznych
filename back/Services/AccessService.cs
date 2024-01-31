using Microsoft.AspNetCore.Cors.Infrastructure;
using projekt.Db.Repository;
using projekt.Db.Repository.Interfaces;
using projekt.Models.Dtos;
using projekt.Services.Interfaces;

namespace projekt.Services;
public class AccessService : IAccessService
{
    private readonly ITimeOutRepository _timeoutsRepository;
    private readonly ICryptoService _cryptoService;
    private readonly IActivityRepository _activityRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IConfiguration _configuration;

    public AccessService(
            ITimeOutRepository timeoutsRepository, 
            ICryptoService cryptoService,
            IActivityRepository activityRepository, 
            ISessionRepository sessionRepository,
            IConfiguration configuration)
    {
        _timeoutsRepository = timeoutsRepository;
        _cryptoService = cryptoService;
        _activityRepository = activityRepository;
        _sessionRepository = sessionRepository;
        _configuration = configuration;
    }

    public bool ShouldReplayToOrigin(string origin)
    {   
        if(_timeoutsRepository.isTimeOut(origin)) return false;
        var allowedTimeSpanInMinutes = _configuration.GetValue<int>("ClassConfig:AccessService:AllowedTimeSpanInMinutes");
        var timeOutInMinutes = _configuration.GetValue<int>("ClassConfig:AccessService:TimeOutInMinutes");
        var allowedCount = _configuration.GetValue<int>("ClassConfig:AccessService:AllowedCount");
        var count = _activityRepository.GetAcitivityCountForLastNMinutes(origin, allowedTimeSpanInMinutes);
        if(count > allowedCount) _timeoutsRepository.setTimeOut(origin, timeOutInMinutes);
        return count <= allowedCount;
    }

    public bool ShouldReplayToToken(Token token)
    {
        return true;
    }

    public int GetUserForToken(Token token)
    {
        return _sessionRepository.GetUserIdForSession(token.SessionId);
    }

    public int GetUserId(Token token)
    {
        return _sessionRepository.GetUserIdForSession(token.SessionId);
    }

    public bool VerifyToken(Token token)
    {
        //Console.WriteLine("token is not null:" + (token != null));
        if (token == null) return false;
        //Console.WriteLine("token is not expired:" + (token.ExpirationDate > DateTime.Now));
        if (token.ExpirationDate < DateTime.Now) return false;
        //Console.WriteLine("token is valid:" + _cryptoService.VerifyToken(token));
        if(!_cryptoService.VerifyToken(token)) return false;
        //Console.WriteLine("token is valid: Awsome");
        return true;
    }

    public Token GetToken(int usedId)
    {
        var maxSessions = _configuration.GetValue<int>("ClassConfig:SessionService:SessionDurtionInMinutes");
        var expirationDate = DateTime.Now.AddMinutes(maxSessions);
        var ssessionId =_sessionRepository.CreateSession(usedId, expirationDate);
        Token token = _cryptoService.GenerateToken(usedId, ssessionId, expirationDate);
        return token;
    }
}

