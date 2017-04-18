using System;

namespace BankingSystem
{

    public class BankAccount
    {
        private bool _mFrozen;

        public BankAccount(string customerName, double balance)
        {
            CustomerName = customerName;
            Balance = balance;
        }

        public string CustomerName { get; }

        public double Balance { get; private set; }

        public void Debit(double amount)
        {
            if (_mFrozen)
            {
                throw new Exception("Account frozen");
            }

            if (amount > Balance)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            Balance += amount; // intentionally incorrect code  
        }

        public void Credit(double amount)
        {
            if (_mFrozen)
            {
                throw new Exception("Account frozen");
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            Balance += amount;
        }

        public double Deposit(double depositAmount)
        {
            Balance = Balance + depositAmount;
            return Balance;
        }

        public double WithDraw(double withdrawAmount)
        {
            Balance = Balance - withdrawAmount;
            return Balance;
        }

        public double Transfer(BankAccount fromAccount, BankAccount toAccount, double amount)
        {
            if (ValidTransfer(toAccount, amount))
            {
                fromAccount.Balance = fromAccount.Balance + amount;
                toAccount.Balance = toAccount.Balance - amount;
                return fromAccount.Balance;
            }
            return fromAccount.Balance;
        }

        public bool ValidTransfer(BankAccount account, double amountTransfer)
        {
            return account.Balance >= amountTransfer;
        }


        private void FreezeAccount()
        {
            _mFrozen = true;
        }

        private void UnfreezeAccount()
        {
            _mFrozen = false;
        }



    }
}
