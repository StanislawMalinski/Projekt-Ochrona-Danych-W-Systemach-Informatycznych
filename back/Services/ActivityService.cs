
using projekt.Models.Enums;
using projekt.Models.Dtos;
using projekt.Db.Repository;
namespace projekt.Serivces;

public class ActivityService : IActivityService
{
    private readonly IActivityRepository _activityRepository;
    
    public ActivityService(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public void LogActivity(ActivityType type,  bool success)
    {
        var activity = new Activity
        {
            Id = 0,
            Date = DateTime.Now,
            IP = "",
            Type = type,
            Success = success
        };
        _activityRepository.LogActivity(activity);
    }
}