using ATM_Machine.Helpers;

namespace ATM_Machine
{
    public interface IATM
    {
        ProcessResult LogIn(string accountNum, string pin);
        ProcessResult SendMoney(decimal amount, string accountNum);
        ProcessResult Withdraw(decimal amount);
    }
}