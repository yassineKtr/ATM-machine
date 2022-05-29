using ATM_machine.Readers;
using ATM_Machine.Helpers;

namespace ATM_Machine
{
    public class ATM : IATM
    {
        private Account _currentAccount;
        private readonly IReadAccount _accountReader;
        public ATM(IReadAccount accountReader)
        {
            _currentAccount = null;
            _accountReader = accountReader;
        }
        public ProcessResult LogIn(string accountNum, string pin)
        {
            var log = new ProcessResult();
            var account = _accountReader.GetAccount(accountNum);
            if (account == null)
            {
                log.Result = false;
                log.Message = "User not found";
                return log;
            }
            if ((pin.Length != 4 && pin.Length != 6) || pin.Any(x => !char.IsDigit(x)))
            {
                log.Result = false;
                log.Message = "Invalid pin";
                return log;
            }
            _currentAccount = account;
            log.Result = true;
            return log;
        }
        public ProcessResult Withdraw(decimal amount)
        {
            var log = new ProcessResult();
            if (_currentAccount == null)
            {
                log.Result = false;
                log.Message = "User not logged in";
                return log;
            }
            if (amount % 100 != 0)
            {
                log.Result = false;
                log.Message = "Invalid amount";
                return log;
            }
            if (amount >= _currentAccount.AccountBalance)
            {
                log.Result = false;
                log.Message = "Insufficient funds";
                return log;
            }
            _currentAccount.AccountBalance -= amount;
            log.Result = true;
            return log;
        }
        public ProcessResult SendMoney(decimal amount, string accountNum)
        {
            var log = new ProcessResult();
            if (_currentAccount == null)
            {
                log.Result = false;
                log.Message = "User not logged in";
                return log;
            }
            if (accountNum.Length != 6 || accountNum.Any(x => !char.IsDigit(x)))
            {
                log.Result = false;
                log.Message = "Account number not valid";
                return log;
            }
            var receiver = _accountReader.GetAccount(accountNum);
            if (receiver == null)
            {
                log.Result = false;
                log.Message = "Receiver not found";
                return log;
            }
            if (amount >= _currentAccount.AccountBalance)
            {
                log.Result = false;
                log.Message = "Insufficient funds";
                return log;
            }
            _currentAccount.AccountBalance -= amount;
            if (receiver.Nationality == _currentAccount.Nationality)
            {
                receiver.AccountBalance += amount;
            }
            if (receiver.Nationality != _currentAccount.Nationality)
            {
                receiver.AccountBalance += CurrencyConverter.Convert(receiver.Nationality, amount);
            }
            log.Result = true;
            return log;
        }

    }
}
