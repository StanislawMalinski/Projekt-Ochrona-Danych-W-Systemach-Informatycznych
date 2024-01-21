using projekt.Models.Enums;

namespace projekt.Serivces;

public interface IActivityService {
    public void LogActivity(ActivityType type, bool success);
}