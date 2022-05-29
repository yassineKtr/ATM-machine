using ATM_Machine;

namespace ATM_machine.Readers
{
    public interface IReadAccount
    {
        Account GetAccount(string accountNum);
        List<Account> GetAccounts();
    }
}