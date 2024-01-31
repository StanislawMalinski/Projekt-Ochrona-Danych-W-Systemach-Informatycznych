using projekt.Models.Dtos;
using projekt.Models.Enums;

namespace projekt.Services.Interfaces;

public interface IActivityService {
    public void LogActivity(ActivityType type, string AssociatedEmailOrOrign, bool success, bool origin = false);
    public void LogActivity(ActivityType type, string AssociatedEmailOrAccountNumber, string origin, bool success);
    public List<Activity> GetActivities(string email);
}