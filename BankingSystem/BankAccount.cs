using System;

namespace BankingSystem
{

    public class BankAccount
    {
        public BankAccount(string customerName, double balance)
        {
            CustomerName = customerName;
            Balance = balance;
        }

        public string CustomerName { get; }

        public double Balance { get; private set; }

        public double Deposit(double depositAmount)
        {
            if (depositAmount < 0)
                throw new Exception();
            if (depositAmount == 0)
                throw new Exception("Deposit amount is an invalid since is 0");
            Balance += depositAmount;
            return Balance;
        }

        public double WithDraw(double withdrawAmount)
        {
            if (withdrawAmount > Balance)
                throw new Exception();
            if (withdrawAmount == 0)
                throw new Exception("Withdraw amount is 0");
            Balance = Balance - withdrawAmount;
            return Balance;
        }

        public double Transfer(BankAccount toAccount, double amount)
        {
            if (ValidTransfer(toAccount, amount))
            {
                this.Balance = this.Balance - amount;
                toAccount.Balance = toAccount.Balance + amount;
                return this.Balance;
            }
            return this.Balance;
        }

        public bool ValidTransfer(BankAccount account, double transferAmount)
        {
            return account.Balance >= transferAmount && transferAmount > 0 && !(transferAmount < 0);
        }
    }
}
