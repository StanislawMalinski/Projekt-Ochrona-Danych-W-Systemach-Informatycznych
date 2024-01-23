
using projekt.Models.Dtos;
namespace projekt.Db.Repository
{
    public interface IActivityRepository
    {
        public Activity LogActivity(Activity activity);
        public List<Activity> GetActivities(string email);
    }
}