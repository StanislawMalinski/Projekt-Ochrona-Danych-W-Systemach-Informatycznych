using Microsoft.AspNetCore.Cors.Infrastructure;
using projekt.Db.Repository;
using projekt.Db.Repository.Interfaces;
using projekt.Services.Interfaces;

namespace projekt.Services;
public class AccessService : IAccessService
{
    private readonly ITimeOutRepository _timeoutsRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly IConfiguration _configuration;
    private CorsPolicy _corsPolicy;

    public AccessService(ITimeOutRepository timeoutsRepository, IActivityRepository activityRepository, IConfiguration configuration)
    {
        _timeoutsRepository = timeoutsRepository;
        _activityRepository = activityRepository;
        _configuration = configuration;
        _corsPolicy = new CorsPolicy();
    }
    public bool ShouldReplay(string origin)
    {   
        if(_timeoutsRepository.isTimeOut(origin)) return false;
        var timeOutInMinutes = _configuration.GetValue<int>("AccessService:TimeOutInMinutes");
        var allowedCount = _configuration.GetValue<int>("AccessService:AllowedCount");
        var count = _activityRepository.GetAcitivityCountForLastNMinutes(origin, timeOutInMinutes);
        if(count > allowedCount) _timeoutsRepository.setTimeOut(origin, timeOutInMinutes);
        return count <= allowedCount;
    }
}

