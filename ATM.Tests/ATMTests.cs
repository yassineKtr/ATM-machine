using ATM.Tests;
using Xunit;
namespace ATM_Machine
{
    public class ATMTests : IClassFixture<AccountFixture>
    {
        private readonly AccountFixture _accountFixture;
        public ATMTests(AccountFixture testFixture)
        {
            _accountFixture = testFixture;
            _accountFixture.Atm.Logout();
        }
        [Fact]
        public void CannotLogInIfUserNotExists()
        {
            var res = _accountFixture.Atm.LogIn("100001", "1234", _accountFixture.Account);
            Assert.Equal("User not found", res?.Message);
            Assert.False(res?.Result);
        }
        [Fact]
        public void CannotLogInIfPinInvalid()
        {
            var res1 = _accountFixture.Atm.LogIn("000001", "12345", _accountFixture.Account);
            Assert.False(res1?.Result);
            Assert.Equal("Invalid pin", res1?.Message);
            var res2 = _accountFixture.Atm.LogIn("000001", "a234", _accountFixture.Account);
            Assert.False(res2?.Result);
            Assert.Equal("Invalid pin", res2?.Message);
        }
        [Fact]
        public void CanLogInIfPinValidANdUserExists()
        {
            var res = _accountFixture.Atm.LogIn("000001", "1234", _accountFixture.Account);
            Assert.Null(res?.Message);
            Assert.True(res?.Result);
        }
        [Fact]
        public void CannotWithdrawIfNotLoggedIn()
        {
            var res = _accountFixture.Atm.Withdraw(100);
            Assert.False(res?.Result);
            Assert.Equal("User not logged in", res?.Message);
        }
        [Fact]
        public void CannotWithdrawIfNotEnoughMoney()
        {
            _accountFixture.Atm.LogIn("000001", "1234", _accountFixture.Account);
            var res = _accountFixture.Atm.Withdraw(30000);
            Assert.False(res?.Result);
            Assert.Equal("Insufficient funds", res?.Message);
        }
        [Fact]
        public void CannotWithdrawIfAmontNotMultipleOfHundred()
        {
            _accountFixture.Atm.LogIn("000001", "1234", _accountFixture.Account);
            var res = _accountFixture.Atm.Withdraw(201);
            Assert.False(res?.Result);
            Assert.Equal("Invalid amount", res?.Message);
        }
        [Fact]
        public void CanWithdrawIfEnoughMoneyAmountValidAndLoggedIn()
        {
            _accountFixture.Atm.LogIn("000001", "1234", _accountFixture.Account);
            var res = _accountFixture.Atm.Withdraw(1000);
            Assert.True(res?.Result);
            Assert.Null(res?.Message);
            var account = _accountFixture.Account.FirstOrDefault(x => x.Number == "000001");
            Assert.Equal(2000, account?.Balance);
        }
        [Fact]
        public void CannotSendMoneyIfNotLoggedIn()
        {
            var result = _accountFixture.Atm.SendMoney(1000, "000000", _accountFixture.Account);
            Assert.False(result?.Result);
            Assert.Equal("User not logged in", result?.Message);
        }
        [Fact]
        public void CannotSendMoneyIfReceiverAccountNumberNotValid()
        {
            _accountFixture.Atm.LogIn("000001", "1234", _accountFixture.Account);
            var res1 = _accountFixture.Atm.SendMoney(1000, "00000", _accountFixture.Account);
            Assert.False(res1?.Result);
            Assert.Equal("Account number not valid", res1?.Message);
            var res2 = _accountFixture.Atm.SendMoney(1000, "00000a", _accountFixture.Account);
            Assert.False(res2?.Result);
        }
        [Fact]
        public void CannotSendMoneyIfReceiverDoesNotExist()
        {
            _accountFixture.Atm.LogIn("000001", "1234", _accountFixture.Account);
            var result = _accountFixture.Atm.SendMoney(1000, "000000", _accountFixture.Account);
            Assert.False(result?.Result);
            Assert.Equal("Receiver not found", result?.Message);
        }
        [Fact]
        public void CannotSendMoneyNationalyIfMoneyNotEnough()
        {
            _accountFixture.Atm.LogIn("000001", "1234", _accountFixture.Account);
            var res = _accountFixture.Atm.SendMoney(300000, "000002", _accountFixture.Account);
            Assert.False(res?.Result);
            Assert.Equal("Insufficient funds", res?.Message);
        }
        [Fact]
        public void CanSendMoneyNationaly()
        {
            _accountFixture.Atm.LogIn("000002", "0000", _accountFixture.Account);
            var res = _accountFixture.Atm.SendMoney(1000, "000001", _accountFixture.Account);
            Assert.True(res?.Result);
            Assert.Null(res?.Message);
            var account = _accountFixture.Account.FirstOrDefault(x => x.Number == "000002");
            Assert.Equal(1000, account?.Balance);
            var receiver = _accountFixture.Account.FirstOrDefault(x => x.Number == "000001");
            Assert.Equal(3000, receiver?.Balance);
        }
        [Fact]
        public void CanSendMoneyInterNationalyWithConversionFromHomeToAbroad()
        {
            _accountFixture.Atm.LogIn("000003", "1111", _accountFixture.Account);
            var receiver = _accountFixture.Account.FirstOrDefault(x => x.Number == "000004");
            var newAmount = receiver?.Balance + CurrencyConverter.Convert(receiver.Nationality, 1000);
            var res = _accountFixture.Atm.SendMoney(1000, "000004", _accountFixture.Account);
            Assert.True(res?.Result);
            var account = _accountFixture.Account.FirstOrDefault(x => x.Number == "000003");
            Assert.Equal(1000, account?.Balance);
            Assert.Equal(newAmount, receiver?.Balance);
        }
        [Fact]
        public void CanSendMoneyInterNationalyWithConversionFromAbroadToHome()
        {
            _accountFixture.Atm.LogIn("000005", "0202", _accountFixture.Account);
            var receiver = _accountFixture.Account.FirstOrDefault(x => x.Number == "000001");
            var newAmount = receiver?.Balance + CurrencyConverter.Convert(receiver.Nationality, 1000);
            var res = _accountFixture.Atm.SendMoney(1000, "000001", _accountFixture.Account);
            Assert.True(res?.Result);
            var account = _accountFixture.Account.FirstOrDefault(x=>x.Number =="000005");
            Assert.Equal(1000, account?.Balance);
            Assert.Equal(newAmount, receiver?.Balance);
        }       
    }
}