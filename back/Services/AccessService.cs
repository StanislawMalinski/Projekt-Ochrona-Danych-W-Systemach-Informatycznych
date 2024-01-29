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

    public AccessService(ITimeOutRepository timeoutsRepository, IActivityRepository activityRepository, IConfiguration configuration)
    {
        _timeoutsRepository = timeoutsRepository;
        _activityRepository = activityRepository;
        _configuration = configuration;
    }

    public bool ShouldReplay(string origin)
    {   
        if(_timeoutsRepository.isTimeOut(origin)) return false;
        var allowedTimeSpanInMinutes = _configuration.GetValue<int>("ClassConfig:AccessService:AllowedTimeSpanInMinutes");
        var timeOutInMinutes = _configuration.GetValue<int>("ClassConfig:AccessService:TimeOutInMinutes");
        var allowedCount = _configuration.GetValue<int>("ClassConfig:AccessService:AllowedCount");
        var count = _activityRepository.GetAcitivityCountForLastNMinutes(origin, allowedTimeSpanInMinutes);
        if(count > allowedCount) _timeoutsRepository.setTimeOut(origin, timeOutInMinutes);
        return count <= allowedCount;
    }
}

