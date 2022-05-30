using ATM_Machine;
using Microsoft.Extensions.DependencyInjection;
namespace ATM.Tests;
public class AccountFixture : TestFixture
{
    public readonly IATM Atm;
    public List<Account> Account = new List<Account>
            {
                new Account
                {
                    AccountNumber = "000001",
                    AccountBalance = 2000,
                    AccountPin = "1234",
                    Nationality ="nat"
                },
                new Account
                {
                    AccountNumber = "000002",
                    AccountBalance = 2000,
                    AccountPin = "0000",
                    Nationality ="nat"
                },
                new Account
                {
                    AccountNumber = "000003",
                    AccountBalance = 2000,
                    AccountPin = "1111",
                    Nationality = "nat"
                },
                new Account
                {
                    AccountNumber = "000004",
                    AccountBalance = 2000,
                    AccountPin = "0101",
                    Nationality = "euro"
                },
                new Account
                {
                    AccountNumber = "000005",
                    AccountBalance = 2000,
                    AccountPin = "0202",
                    Nationality = "usd"
                },
            };
    public AccountFixture()
    {
        Atm = ServiceProvider.GetService<IATM>();
    }
}
