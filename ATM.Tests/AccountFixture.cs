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
                    Number = "000001",
                    Balance = 2000,
                    Pin = "1234",
                    Nationality ="nat"
                },
                new Account
                {
                    Number = "000002",
                    Balance = 2000,
                    Pin = "0000",
                    Nationality ="nat"
                },
                new Account
                {
                    Number = "000003",
                    Balance = 2000,
                    Pin = "1111",
                    Nationality = "nat"
                },
                new Account
                {
                    Number = "000004",
                    Balance = 2000,
                    Pin = "0101",
                    Nationality = "euro"
                },
                new Account
                {
                    Number = "000005",
                    Balance = 2000,
                    Pin = "0202",
                    Nationality = "usd"
                },
            };
    public AccountFixture()
    {
        Atm = ServiceProvider.GetService<IATM>();
    }
}
