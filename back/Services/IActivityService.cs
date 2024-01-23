using projekt.Models.Dtos;
using projekt.Models.Enums;

namespace projekt.Serivces;

public interface IActivityService {
    public void LogActivity(ActivityType type, bool success);
    public void LogActivity(ActivityType type, string AssociatedEmail, bool success);
    public List<Activity> GetActivities(string email);
}