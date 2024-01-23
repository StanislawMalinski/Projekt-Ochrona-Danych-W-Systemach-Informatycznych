
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
             _bankDbContext.Activities.Add(activity);
             _bankDbContext.SaveChanges();
            return activity;
        }

        public  List<Activity> GetActivities(string email)
        {
            return _bankDbContext.Activities
                .Where(a => a.AssociatedEmail == email).ToList();
        }

    }
}