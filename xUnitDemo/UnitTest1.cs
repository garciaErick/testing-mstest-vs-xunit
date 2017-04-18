using Xunit;

namespace xUnitDemo
{
    public class Class1
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, 2+2);
        }


        [Fact]
        public void Withdraw_AmountGreaterThanBalance_ExceptionThrown()
        {
            //TODO:
        }

        [Fact]
        public void Withdraw_AmmountIsZero_ExceptionThrown()
        {
        }

        [Fact]
        public void Withdraw_AmountLesThanBalance_ReturnNewBalance()
        {
        }

        [Fact]
        public void Deposit_NegativeAmmount_ExceptionThrown()
        {
        }

        [Fact]
        public void Deposit_AmmountIsZero_ExceptionThrown()
        {
        }

        [Fact]
        public void Deposit_PositiveAmmount_ReturnNewBalance()
        {
        }

        [Fact]
        public void ValidTransfer_IsValidTransfer_ReturnTrue()
        {
        }

        [Fact]
        public void ValidTransfer_TransferAmountIsZero_ReturnFalse()
        {
        }

        [Fact]
        public void ValidTransfer_TransferIsGreaterThanBalance_ReturnFalse()
        {
        }

        [Fact]
        public void ValidTransfer_TransferIsNegative_ReturnFalse()
        {
        }

        [Fact]
        public void Transfer_AmountIsZero_ReturnUnchangedBalance()
        {
        }

        [Fact]
        public void Transfer_AmountIsNegative_ReturnUnchangedBalance()
        {
        }

        [Fact]
        public void Transfer_AmountIsGreaterThanBalance_ReturnUnchangedBalance()
        {
        }

        [Fact]
        public void Transfer_AmountIsValid_ReturnNewBalance()
        {
        }

        [Fact]
        public void Transfer_TransferIsDone_ToAccountBalanceIsCorrect()
        {
        }

        [Fact]
        public void Transfer_TransferIsNotDone_ToAccountBalanceIsUnchanged()
        {
        }
    }
}