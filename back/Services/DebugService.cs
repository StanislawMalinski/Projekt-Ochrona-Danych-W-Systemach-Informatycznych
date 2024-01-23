using projekt.Db.Repository;
using projekt.Models.Dtos;
using projekt.Models.Enums;

namespace projekt.Serivces;

public class DebugService : IDebugSerivce
{
    private readonly IActivityRepository _activityRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ITransferRepository _transferRepository;
    private readonly Random _random;

    public DebugService(IActivityRepository activityRepository, IAccountRepository accountRepository, ITransferRepository transferRepository)
    {
        _accountRepository = accountRepository;
        _activityRepository = activityRepository;
        _transferRepository = transferRepository;
        _random = new Random(1);
    }

    public void LogActivity(Activity activity)
    {
        if (!activity.Success ||
            activity.Origin != "") 
            Console.WriteLine(activityToLog(activity));
        
    }

    public void LogMessage(string recipient, string message)
    {
        Console.WriteLine(messageToLog(recipient, message));
    }

    private string activityToLog(Activity activity){
        return $"[{activity.Date}] Type: {activity.Type} |Ended with success: {activity.Success} |Origin of the request: {activity.Origin} |Associeted email: {activity.AssociatedEmail} |";
    }

    private string messageToLog(string recipient, string message){
        return $"[{DateTime.Now}] Type: Message |Recipient: {recipient} |Content: {message} |";
    }

    public void SeedDatabase()
    {
        List<string> names = new List<string> {"stasio", "jasio", "zosio", "tomaszo", "anio", "andrzejo", "natalko", "krzysio", "asio"};
        Account account;
        List<Account> accounts = new List<Account>();
        Transfer transfer;
        foreach (string name in names)
        {  
           account = new Account{
                Name = name,
                Email = name + "@gmail.com",
                Password = "1234",
                Balance = 100M
            };
           _accountRepository.Register(account);
           _activityRepository.LogActivity(new Activity
           {
               AssociatedEmail = account.Email,
               Date = DateTime.Now,
               Origin = "",
               Success = true,
               Type = ActivityType.Register
           });
        }
        foreach (Account account1 in accounts)
        {
            for (int i = 0; i < 10; i++)
            {
                Account account2 = accounts[_random.Next(0, accounts.Count)];
                if (account1 != account2)
                {
                    transfer = new Transfer
                    {
                        Value = _random.Next(1, 10),
                        AccountNumber = account2.AccountNumber,
                        RecipentAccountNumber = account1.AccountNumber,
                        Date = DateTime.Now,
                        Title = "test"
                    };
                    _transferRepository.NewTransfer(transfer);
                    _activityRepository.LogActivity(new Activity
                    {
                        AssociatedEmail = account1.Email,
                        Date = DateTime.Now,
                        Origin = "",
                        Success = true,
                        Type = ActivityType.NewTransfer
                    });
                }
            }
        }
    }
}

