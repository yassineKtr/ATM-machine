using ATM_Machine.Helpers;

namespace ATM_Machine
{
    public interface IATM
    {
        ProcessResult LogIn(string accountNum, string pin, IList<Account> accounts);
        ProcessResult SendMoney(decimal amount, string accountNum, IList<Account> accounts);
        ProcessResult Withdraw(decimal amount);
        void Logout();
    }
}