
using projekt.Models.Dtos;
using System.Collections.Generic;

namespace projekt.Db.Repository
{
    public interface IActivityRepository
    {
        public int GetAcitivityCountForLastNMinutes(string origin, int minutes);
        public List<Activity> GetActivities(string email);
        List<string> GetRelevantOrigins(string email);
        public Activity LogActivity(Activity activity);
    }
}