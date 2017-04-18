using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsTestDemo
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void Withdraw_AmountGreaterThanBalance_ExceptionThrown()
        {
        }

        [TestMethod]
        public void Withdraw_AmmountIsZero_ExceptionThrown()
        {
        }

        [TestMethod]
        public void Withdraw_AmountLesThanBalance_ReturnNewBalance()
        {
        }

        [TestMethod]
        public void Deposit_NegativeAmmount_ExceptionThrown()
        {
        }

        [TestMethod]
        public void Deposit_AmmountIsZero_ExceptionThrown()
        {
        }

        [TestMethod]
        public void Deposit_PositiveAmmount_ReturnNewBalance()
        {
        }

        [TestMethod]
        public void ValidTransfer_IsValidTransfer_ReturnTrue()
        {
        }

        [TestMethod]
        public void ValidTransfer_TransferAmountIsZero_ReturnFalse()
        {
        }

        [TestMethod]
        public void ValidTransfer_TransferIsGreaterThanBalance_ReturnFalse()
        {
        }

        [TestMethod]
        public void ValidTransfer_TransferIsNegative_ReturnFalse()
        {
        }

        [TestMethod]
        public void Transfer_AmountIsZero_ReturnUnchangedBalance()
        {
        }

        [TestMethod]
        public void Transfer_AmountIsNegative_ReturnUnchangedBalance()
        {
        }

        [TestMethod]
        public void Transfer_AmountIsGreaterThanBalance_ReturnUnchangedBalance()
        {
        }

        [TestMethod]
        public void Transfer_AmountIsValid_ReturnNewBalance()
        {
        }

        [TestMethod]
        public void Transfer_TransferIsDone_ToAccountBalanceIsCorrect()
        {
        }

        [TestMethod]
        public void Transfer_TransferIsNotDone_ToAccountBalanceIsUnchanged()
        {
        }
    }
}
