
using projekt.Models.Enums;
using projekt.Models.Dtos;
using projekt.Db.Repository;
using projekt.Db.Repository.Interfaces;
using projekt.Services.Interfaces;

namespace projekt.Services;

public class ActivityService : IActivityService
{
    private readonly IActivityRepository _activityRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IDebugSerivce _debug_service;
    
    public ActivityService(IActivityRepository activityRepository, 
                            IAccountRepository accountRepository, 
                            IDebugSerivce debug_service)
    {
        _activityRepository = activityRepository;
        _accountRepository = accountRepository;
        _debug_service = debug_service;
    }

    public void LogActivity(ActivityType type, string AssociatedEmailOrAccountNumber, string origin, bool success)
    {
        var email = GetEmail(AssociatedEmailOrAccountNumber);
        var activity = new Activity
        {
            Id = 0,
            Date = DateTime.Now,
            AssociatedEmail = email,
            Origin = origin,
            Type = type,
            Success = success
        };
        publishActivity(activity);
    }

    public void LogActivity(ActivityType type, string AssociatedEmailOrAccountNumber, bool success)
    {
        var email = GetEmail(AssociatedEmailOrAccountNumber);
        var activity = new Activity
        {
            Id = 0,
            Date = DateTime.Now,
            AssociatedEmail = email,
            Origin = "",
            Type = type,
            Success = success
        };
        publishActivity(activity);
    }

    private string GetEmail(string AssociatedEmailOrAccountNumber)
    {
        if (AssociatedEmailOrAccountNumber.Contains('@'))
            return AssociatedEmailOrAccountNumber;
        var account = _accountRepository.GetAccount(AssociatedEmailOrAccountNumber);
        return account != null ? account.Email : AssociatedEmailOrAccountNumber;
    }

    private void publishActivity(Activity activity)
    {
        _debug_service.LogActivity(activity);
        _activityRepository.LogActivity(activity);
    }

    public List<Activity> GetActivities(string email)
    {
        return _activityRepository.GetActivities(email);
    }
}