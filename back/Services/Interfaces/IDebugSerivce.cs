using projekt.Models.Dtos;

namespace projekt.Services.Interfaces;

public interface IDebugSerivce
{
    public void LogActivity(Activity activity);
    public void LogMessage(string recipient, string message);
    public void SeedDatabase();
}