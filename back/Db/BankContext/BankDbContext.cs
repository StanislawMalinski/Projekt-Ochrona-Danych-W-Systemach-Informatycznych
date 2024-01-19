using Microsoft.EntityFrameworkCore;
using projekt.Models.Dtos;

namespace projekt.Db.BankContext
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options): base(options){}
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}