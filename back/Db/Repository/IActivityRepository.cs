
using projekt.Models.Dtos;
namespace projekt.Db.Repository
{
    public interface IActivityRepository
    {
        public Activity GetActivity(int activityId);
        public Activity LogActivity(Activity activity);
        public List<Activity> GetActivities();
    }
}