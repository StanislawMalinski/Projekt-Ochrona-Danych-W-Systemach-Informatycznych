using projekt.Models.Dtos;
using projekt.Models.Enums;

namespace projekt.Services.Interfaces;

public interface IActivityService {
    public void LogActivity(ActivityType type, string AssociatedEmail, bool success);
    public void LogActivity(ActivityType type, string AssociatedEmailOrAccountNumber, string origin, bool success);
    public List<Activity> GetActivities(string email);
}