using projekt.Models.Enums;

namespace projekt.Serivces;

public interface IActivityService {
    public void LogActivity(ActivityType type, HttpContext context, bool success);
}