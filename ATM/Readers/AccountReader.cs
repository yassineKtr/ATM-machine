using ATM_Machine;

namespace ATM_machine.Readers
{
    public class AccountReader : IReadAccount
    {
        private readonly IDataRetriever _data;
        public AccountReader(IDataRetriever data)
        {
            _data = data;
        }
        public Account? GetAccount(string accountNum) => _data.GetAvailableAccounts().FirstOrDefault(x => x.AccountNumber == accountNum);
        public List<Account> GetAccounts() => _data.GetAvailableAccounts();
    }
}
