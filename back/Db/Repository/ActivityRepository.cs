
using projekt.Models.Dtos;
using projekt.Db.Repository;
using projekt.Db.BankContext;
using projekt.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace projekt.Db.Repository
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly BankDbContext  _bankDbContext;

        public ActivityRepository(BankDbContext context)
        {
             _bankDbContext = context;
        }

        public Activity LogActivity(Activity activity)
        {
            cleanUp();
             _bankDbContext.Activities.Add(activity);
             _bankDbContext.SaveChanges();
            return activity;
        }

        public  List<Activity> GetActivities(string email)
        {
            cleanUp();
            return _bankDbContext.Activities
                .Where(a => a.AssociatedEmail == email)
                .ToList();
        }

        public int GetAcitivityCountForLastNMinutes(string origin, int minutes)
        {
            cleanUp();
            var date = DateTime.Now.AddMinutes(-1 * minutes);
            var count = _bankDbContext.Activities
                .Where(a => a.Origin == origin && a.Date > date)
                .Count();
            return count;
        }

        private void cleanUp(){
            _bankDbContext.Activities
                .Where(a => a.Date < DateTime.Now.AddDays(-7))
                .ToList()
                .ForEach(a => _bankDbContext.Activities.Remove(a));
            _bankDbContext.SaveChanges();
        }
    }
}