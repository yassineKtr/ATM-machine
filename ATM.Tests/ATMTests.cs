using ATM_machine.Readers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ATM_Machine
{
    public class ATMTests : IClassFixture<TestFixture>
    {
        private ServiceProvider _serviceProvider;
        private readonly IATM _atm;
        private readonly IReadAccount _accountReader;
        public ATMTests(TestFixture testFixture)
        {
            _serviceProvider = testFixture.ServiceProvider;
            _atm = _serviceProvider.GetService<IATM>();
            _accountReader = _serviceProvider.GetService<IReadAccount>();
        }
       
        [Fact]
        public void CannotLogInIfUserNotExists()
        {
            var res = _atm?.LogIn("100001", "1234");
            Assert.Equal("User not found", res?.Message);
            Assert.False(res?.Result);
        }
        [Fact]
        public void CannotLogInIfPinInvalid()
        {
            var res1 = _atm?.LogIn("000001", "12345");
            Assert.False(res1?.Result);
            Assert.Equal("Invalid pin", res1?.Message);
            var res2 = _atm?.LogIn("000001", "a234");
            Assert.False(res2?.Result);
            Assert.Equal("Invalid pin", res2?.Message);
        }
        [Fact]
        public void CanLogInIfPinValidANdUserExists()
        {
            var res = _atm?.LogIn("000001", "1234");
            Assert.Null(res?.Message);
            Assert.True(res?.Result);
        }
        [Fact]
        public void CannotWithdrawIfNotLoggedIn()
        {
            var res = _atm?.Withdraw(100);
            Assert.False(res?.Result);
            Assert.Equal("User not logged in", res?.Message);
        }
        [Fact]
        public void CannotWithdrawIfNotEnoughMoney()
        {
            _atm?.LogIn("000001", "1234");
            var res = _atm?.Withdraw(30000);
            Assert.False(res?.Result);
            Assert.Equal("Insufficient funds", res?.Message);
        }
        [Fact]
        public void CannotWithdrawIfAmontNotMultipleOfHundred()
        {
            _atm?.LogIn("000001", "1234");
            var res = _atm?.Withdraw(201);
            Assert.False(res?.Result);
            Assert.Equal("Invalid amount", res?.Message);
        }
        [Fact]
        public void CanWithdrawIfEnoughMoneyAmountValidAndLoggedIn()
        {
            _atm?.LogIn("000001", "1234");
            var res = _atm?.Withdraw(1000);
            Assert.True(res?.Result);
            Assert.Null(res?.Message);
            var account = _accountReader?.GetAccount("000001");
            Assert.Equal(2000, account?.AccountBalance);
        }
        [Fact]
        public void CannotSendMoneyIfNotLoggedIn()
        {
            var result = _atm?.SendMoney(1000, "000000");
            Assert.False(result?.Result);
            Assert.Equal("User not logged in", result?.Message);
        }
        [Fact]
        public void CannotSendMoneyIfReceiverAccountNumberNotValid()
        {
            _atm?.LogIn("000001", "1234");
            var res1 = _atm?.SendMoney(1000, "00000");
            Assert.False(res1?.Result);
            Assert.Equal("Account number not valid", res1?.Message);
            var res2 = _atm?.SendMoney(1000, "00000a");
            Assert.False(res2?.Result);
        }
        [Fact]
        public void CannotSendMoneyIfReceiverDoesNotExist()
        {
            _atm?.LogIn("000001", "1234");
            var result = _atm?.SendMoney(1000, "000000");
            Assert.False(result?.Result);
            Assert.Equal("Receiver not found", result?.Message);
        }
        [Fact]
        public void CannotSendMoneyNationalyIfMoneyNotEnough()
        {
            _atm?.LogIn("000001", "1234");
            var res = _atm?.SendMoney(300000, "000002");
            Assert.False(res?.Result);
            Assert.Equal("Insufficient funds", res?.Message);
        }
        [Fact]
        public void CanSendMoneyNationaly()
        {
            _atm?.LogIn("000002", "0000");
            var res = _atm?.SendMoney(1000, "000001");
            Assert.True(res?.Result);
            Assert.Null(res?.Message);
            var account = _accountReader?.GetAccount("000002");
            Assert.Equal(1000, account?.AccountBalance);
            var receiver = _accountReader?.GetAccount("000001");
            Assert.Equal(3000, receiver?.AccountBalance);
        }
        [Fact]
        public void CanSendMoneyInterNationalyWithConversionFromHomeToAbroad()
        {
            _atm?.LogIn("000003", "1111");
            var receiver = _accountReader?.GetAccount("000004");
            var newAmount = receiver?.AccountBalance + CurrencyConverter.Convert(receiver.Nationality, 1000);
            var res = _atm?.SendMoney(1000, "000004");
            Assert.True(res?.Result);
            var account = _accountReader?.GetAccount("000003");
            Assert.Equal(1000, account?.AccountBalance);
            Assert.Equal(newAmount, receiver?.AccountBalance);
        }
        [Fact]
        public void CanSendMoneyInterNationalyWithConversionFromAbroadToHome()
        {
            _atm?.LogIn("000005", "0202");
            var receiver = _accountReader?.GetAccount("000001");
            var newAmount = receiver?.AccountBalance + CurrencyConverter.Convert(receiver.Nationality, 1000);
            var res = _atm?.SendMoney(1000, "000001");
            Assert.True(res?.Result);
            var account = _accountReader?.GetAccount("000005");
            Assert.Equal(1000, account?.AccountBalance);
            Assert.Equal(newAmount, receiver?.AccountBalance);
        }
    }
}