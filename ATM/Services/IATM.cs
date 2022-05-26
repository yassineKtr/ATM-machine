using ATM_Machine.Helpers;

namespace ATM_Machine
{
    public interface IATM
    {
        Log LogIn(string accountNum, string pin);
        Log SendMoney(decimal amount, string accountNum);
        Log Withdraw(decimal amount);
    }
}