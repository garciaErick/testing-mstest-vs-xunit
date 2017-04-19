using BankingSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MsTestDemo
{
    [TestClass]
    public class MsTest
    {

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Withdraw_AmountGreaterThanBalance_ExceptionThrownv1()
        {
            BankAccount ba = new BankAccount("Eric Camacho", 1000);
            ba.WithDraw(1001);

            //Assert
        }

        [TestMethod]
        public void Withdraw_AmountGreaterThanBalance_ExceptionThrown2()
        {
            BankAccount ba = new BankAccount("Eric Camacho", 1000);
            Exception expectedException = null;

            // Act
            try
            {
                ba.WithDraw(1001);
            }
            catch (Exception ex)
            {
                expectedException = ex;
            }

            // Assert
            Assert.IsNotNull(expectedException);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Withdraw_AmmountIsZero_ExceptionThrown()
        {
            BankAccount ba = new BankAccount("Eric Camacho", 1000);
            ba.WithDraw(0);
        }

        [TestMethod]
        public void Withdraw_AmountLessThanBalance_ReturnNewBalance()
        {
            BankAccount ba = new BankAccount("Camacho", 500);
            Assert.AreEqual(200, ba.WithDraw(300));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Deposit_NegativeAmmount_ExceptionThrown()
        {
            BankAccount ba = new BankAccount("Eric Camacho", 1000);
            ba.Deposit(-1001);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Deposit_AmmountIsZero_ExceptionThrown()
        {
            BankAccount ba = new BankAccount("Eric Camacho", 1000);
            ba.Deposit(0);
        }

        [TestMethod]
        public void Deposit_PositiveAmmount_ReturnNewBalance()
        {
            BankAccount ba = new BankAccount("Eric Camacho", 1000);
            Assert.AreEqual(1500, ba.Deposit(500));
        }

        [TestMethod]
        public void ValidTransfer_IsValidTransfer_ReturnTrue()
        {
            BankAccount ba = new BankAccount("Garcia", 500);
            Assert.IsTrue(ba.ValidTransfer(ba, 50));
        }

        [TestMethod]
        public void ValidTransfer_TransferAmountIsZero_ReturnFalse()
        {
            BankAccount ba = new BankAccount("Garcia", 500);
            Assert.IsFalse(ba.ValidTransfer(ba, 0));
        }

        [TestMethod]
        public void ValidTransfer_TransferIsGreaterThanBalance_ReturnFalse()
        {
            BankAccount ba = new BankAccount("Garcia", 500);
            Assert.IsFalse(ba.ValidTransfer(ba, 600));
        }

        [TestMethod]
        public void ValidTransfer_TransferIsNegative_ReturnFalse()
        {
            BankAccount ba = new BankAccount("Garcia", 500);
            Assert.IsFalse(ba.ValidTransfer(ba, -10));
        }

        [TestMethod]
        public void Transfer_AmountIsZero_ReturnUnchangedBalance()
        {
            BankAccount ba = new BankAccount("Garcia", 500);
            BankAccount ba2 = new BankAccount("Camacho", 520);
            Assert.AreEqual(500, ba.Transfer(ba2, 0));
        }

        [TestMethod]
        public void Transfer_AmountIsNegative_ReturnUnchangedBalance()
        {
            BankAccount ba = new BankAccount("Garcia", 500);
            BankAccount ba2 = new BankAccount("Camacho", 520);
            Assert.AreEqual(500, ba.Transfer(ba2, -10));
        }

        [TestMethod]
        public void Transfer_AmountIsGreaterThanBalance_ReturnUnchangedBalance()
        {
            BankAccount ba = new BankAccount("Garcia", 500);
            BankAccount ba2 = new BankAccount("Camacho", 520);
            Assert.AreEqual(500, ba.Transfer(ba2, 1100));
        }

        [TestMethod]
        public void Transfer_AmountIsValid_ReturnNewBalance()
        {
            BankAccount ba = new BankAccount("Garcia", 500);
            BankAccount ba2 = new BankAccount("Camacho", 520);
            Assert.AreEqual(470, ba.Transfer(ba2, 30));
        }

        [TestMethod]
        public void Transfer_TransferIsDone_ToAccountBalanceIsCorrect()
        {
            BankAccount fromAccount = new BankAccount("Garcia", 500);
            BankAccount toAccount = new BankAccount("Camacho", 520);
            fromAccount.Transfer(toAccount, 30);
            Assert.AreEqual(550, toAccount.Balance);

        }

        [TestMethod]
        public void Transfer_TransferIsNotDone_ToAccountBalanceIsUnchanged()
        {
            BankAccount ba = new BankAccount("Garcia", 500);
            BankAccount ba2 = new BankAccount("Camacho", 20);
            Assert.AreEqual(500, ba.Transfer(ba2, 30));
        }
    }
}
