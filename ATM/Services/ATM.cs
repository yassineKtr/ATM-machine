using ATM_Machine.Helpers;
namespace ATM_Machine
{
    public class ATM : IATM
    {
        private Account _currentAccount;
        public ProcessResult LogIn(string accountNum, string pin, IList<Account> accounts)
        {
            var processResult = new ProcessResult();
            var account = accounts.FirstOrDefault(x => x.Number == accountNum);
            if (account == null)
            {
                processResult.Result = false;
                processResult.Message = "User not found";
                return processResult;
            }
            if ((pin.Length != 4 && pin.Length != 6) || pin.Any(x => !char.IsDigit(x)))
            {
                processResult.Result = false;
                processResult.Message = "Invalid pin";
                return processResult;
            }
            _currentAccount = account;
            processResult.Result = true;
            processResult.Account = account;
            return processResult;
        }
        public ProcessResult Withdraw(decimal amount)
        {
            var processResult = new ProcessResult();
            if (_currentAccount == null)
            {
                processResult.Result = false;
                processResult.Message = "User not logged in";
                return processResult;
            }
            if (amount % 100 != 0)
            {
                processResult.Result = false;
                processResult.Message = "Invalid amount";
                return processResult;
            }
            if (amount >= _currentAccount.Balance)
            {
                processResult.Result = false;
                processResult.Message = "Insufficient funds";
                return processResult;
            }
            _currentAccount.Balance -= amount;
            processResult.Result = true;
            return processResult;
        }
        public ProcessResult SendMoney(decimal amount, string accountNum, IList<Account> accounts)
        {
            var processResult = new ProcessResult();
            if (_currentAccount == null)
            {
                processResult.Result = false;
                processResult.Message = "User not logged in";
                return processResult;
            }
            if (accountNum.Length != 6 || accountNum.Any(x => !char.IsDigit(x)))
            {
                processResult.Result = false;
                processResult.Message = "Account number not valid";
                return processResult;
            }
            var receiver = accounts.FirstOrDefault(x => x.Number ==accountNum);
            if (receiver == null)
            {
                processResult.Result = false;
                processResult.Message = "Receiver not found";
                return processResult;
            }
            if (amount >= _currentAccount.Balance)
            {
                processResult.Result = false;
                processResult.Message = "Insufficient funds";
                return processResult;
            }
            _currentAccount.Balance -= amount;
            if (receiver.Nationality == _currentAccount.Nationality)
            {
                receiver.Balance += amount;
            }
            if (receiver.Nationality != _currentAccount.Nationality)
            {
                receiver.Balance += CurrencyConverter.Convert(receiver.Nationality, amount);
            }
            processResult.Result = true;
            return processResult;
        }
        public void Logout()
        {
            _currentAccount = null;
        }
    }
}
