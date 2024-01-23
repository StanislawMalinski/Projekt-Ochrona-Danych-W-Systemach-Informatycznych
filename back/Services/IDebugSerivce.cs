using projekt.Models.Dtos;

namespace projekt.Serivces;


public interface IDebugSerivce
{
    public void LogActivity(Activity activity);
    public void LogMessage(string recipient, string message);
    public void SeedDatabase();
}