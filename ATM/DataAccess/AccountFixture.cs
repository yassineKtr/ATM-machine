namespace ATM_Machine;
public class AccountFixture : IDataRetriever
{
    public List<Account> _accounts;
    public AccountFixture()
    {
        _accounts = new List<Account>
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
                    Nationality ="nat"
                },
                new Account
                {
                    AccountNumber = "000004",
                    AccountBalance = 2000,
                    AccountPin = "0101",
                    Nationality ="euro"
                },
                new Account
                {
                    AccountNumber = "000005",
                    AccountBalance = 2000,
                    AccountPin = "0202",
                    Nationality ="usd"
                },
            };
    }
    public List<Account> GetAvailableAccounts() => _accounts;
}
