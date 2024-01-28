using projekt.Db.BankContext;
using projekt.Models.Dtos;

public class TimeOutRepository : ITimeOutRepository
{
    private readonly BankDbContext _bankDbContext;
    public TimeOutRepository(BankDbContext context)
    {
        _bankDbContext = context;
    }

    public bool isTimeOut(string origin){
        return _bankDbContext.TimeOuts
            .Where(t => t.Origin == origin)
            .Where(t => t.ExpirationDate > DateTime.Now)
            .Count() > 0;
    }


    public void setTimeOut(string origin, int timeOutInMinutes){
        _bankDbContext.TimeOuts
            .Add(new Timeouts{
                Origin = origin,
                ExpirationDate = DateTime.Now.AddMinutes(timeOutInMinutes)
            });
    }

    public List<string> listOfTimeOuts()
    {
        return _bankDbContext.TimeOuts
            .Where(t => t.ExpirationDate > DateTime.Now)
            .Select(t => t.Origin)
            .ToList();
    }

    public void cleanUp(){
        _bankDbContext.TimeOuts
            .Where(t => t.ExpirationDate < DateTime.Now.AddDays(-7))
            .ToList()
            .ForEach(t => _bankDbContext.TimeOuts.Remove(t));
        _bankDbContext.SaveChanges();
    }
}
