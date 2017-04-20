using BankingSystem;
using System;
using Xunit;
namespace xUnitDemo

{
    public class Class1
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, 2 + 2);
        }


        [Fact]
        public void Withdraw_AmountGreaterThanBalance_ExceptionThrown()
        {
            BankAccount ba1 = new BankAccount("Eric Camacho", 1000);
            Assert.Throws<Exception>(() => ba1.WithDraw(1001));

        }

        [Fact]
        public void Withdraw_AmmountIsZero_ExceptionThrown()
        {
            BankAccount ba1 = new BankAccount("Eric Camacho", 1000);
            Assert.Throws<Exception>(() => ba1.WithDraw(0));
        }

        [Fact]
        public void Withdraw_AmmountIsNegative_ExceptionThrown()
        {
            BankAccount ba1 = new BankAccount("Eric Camacho", 1000);
            Assert.Throws<Exception>(() => ba1.WithDraw(-1));
        }

        [Theory]
        [InlineData(1002)]
        [InlineData(1001)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        public void Withdraw_IncorrectAmount_ExceptionThrown(double amount)
        {
            BankAccount ba1 = new BankAccount("Eric Camacho", 1000);
            Assert.Throws<Exception>(() => ba1.WithDraw(amount));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Withdraw_CaptureExceptionThrown(double amount)
        { 
            BankAccount ba1 = new BankAccount("Eric Camacho", 1000);
           Exception ex= Assert.Throws<Exception>(() => ba1.WithDraw(amount));

            Assert.Equal("Not Valid Amount To Withdraw", ex.Message);
        }

        [Fact]
        public void Withdraw_AmountLesThanBalance_ReturnNewBalance()
        {
            BankAccount bankAc3 = new BankAccount("Camacho", 500);
            Assert.Equal(200, bankAc3.WithDraw(300));
        }

        [Fact]
        public void Deposit_NegativeAmmount_ExceptionThrown()
        {
            BankAccount ba1 = new BankAccount("Eric Camacho", 1000);
            Assert.Throws<Exception>(() => ba1.Deposit(-1001));
        }

        [Fact]
        public void Deposit_AmmountIsZero_ExceptionThrown()
        {
            BankAccount ba1 = new BankAccount("Eric Camacho", 1000);
            Assert.Throws<Exception>(() => ba1.Deposit(0));

        }

        [Fact]
        public void Deposit_PositiveAmmount_ReturnNewBalance()
        {
            BankAccount ba1 = new BankAccount("Eric Camacho", 1000);
            Assert.Equal(1500, ba1.Deposit(500));

        }

        [Fact]
        public void ValidTransfer_IsValidTransfer_ReturnTrue()
        {
            BankAccount ba1 = new BankAccount("Garcia", 500);
            Assert.True(ba1.ValidTransfer(ba1,50));

        }

        [Fact]
        public void ValidTransfer_TransferAmountIsZero_ReturnFalse()
        {
            BankAccount ba1 = new BankAccount("Garcia", 500);
            Assert.False(ba1.ValidTransfer(ba1, 0));
        }

        [Fact]
        public void ValidTransfer_TransferIsGreaterThanBalance_ReturnFalse()
        {
            BankAccount ba1 = new BankAccount("Garcia", 500);
            Assert.False(ba1.ValidTransfer(ba1,600));
        }

        [Fact]
        public void ValidTransfer_TransferIsNegative_ReturnFalse()
        {
            BankAccount ba1 = new BankAccount("Garcia", 500);
            Assert.False(ba1.ValidTransfer(ba1, -10));
        }

        [Fact]
        public void Transfer_AmountIsZero_ReturnUnchangedBalance()
        {
            BankAccount ba1 = new BankAccount("Garcia", 500);
            BankAccount ba2 = new BankAccount("Camacho", 520);
            Assert.Equal(500, ba1.Transfer(ba2, 0));

        }

        [Fact]
        public void Transfer_AmountIsNegative_ReturnUnchangedBalance()
        {
            BankAccount ba1 = new BankAccount("Garcia", 500);
            BankAccount ba2 = new BankAccount("Camacho", 520);
            Assert.Equal(500, ba1.Transfer(ba2, -10));
        }

        [Fact]
        public void Transfer_AmountIsGreaterThanBalance_ReturnUnchangedBalance()
        {
            BankAccount ba1 = new BankAccount("Garcia", 500);
            BankAccount ba2 = new BankAccount("Camacho", 520);
            Assert.Equal(500, ba1.Transfer(ba2, 1100));
        }

        [Fact]
        public void Transfer_AmountIsValid_ReturnNewBalance()
        {
            BankAccount ba1 = new BankAccount("Garcia", 500);
            BankAccount ba2 = new BankAccount("Camacho", 520);
            Assert.Equal(530, ba1.Transfer(ba2, 30));
        }

        [Fact]
        public void Transfer_TransferIsNotDone_ToAccountBalanceIsUnchanged()
        {
            BankAccount ba1 = new BankAccount("Garcia", 500);
            BankAccount ba2 = new BankAccount("Camacho", 20);
            Assert.Equal(500, ba1.Transfer(ba2, 30));
        }
    }
}