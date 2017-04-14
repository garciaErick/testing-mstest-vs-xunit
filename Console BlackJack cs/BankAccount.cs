using System;

namespace BankingSystem
{

    public class BankAccount
    {
        private string m_customerName;

        private double m_balance;

        private bool m_frozen = false;

        private BankAccount()
        {
        }

        public BankAccount(string customerName, double balance)
        {
            m_customerName = customerName;
            m_balance = balance;
        }

        public string CustomerName
        {
            get { return m_customerName; }
        }

        public double Balance
        {
            get { return m_balance; }
        }

        public void Debit(double amount)
        {
            if (m_frozen)
            {
                throw new Exception("Account frozen");
            }

            if (amount > m_balance)
            {
                throw new ArgumentOutOfRangeException("amount");
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount");
            }

            m_balance += amount; // intentionally incorrect code  
        }

        public void Credit(double amount)
        {
            if (m_frozen)
            {
                throw new Exception("Account frozen");
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount");
            }

            m_balance += amount;
        }

        public double Deposit(int depositAmount)
        {
            m_balance = m_balance + (double)depositAmount;
            return m_balance;
        }

        public double WithDraw(int withdrawAmount)
        {
            m_balance = m_balance - (double)withdrawAmount;
            return m_balance;
        }

        public double Transfer(BankAccount fromAccount, BankAccount toAccount, double amount)
        {
            if (validTransfer(toAccount, amount))
            {
                fromAccount.m_balance = fromAccount.m_balance + amount;
                toAccount.m_balance = toAccount.m_balance - amount;
                return fromAccount.m_balance;
            }
            else
               return fromAccount.m_balance;

        }

        public Boolean validTransfer(BankAccount account, double amountTransfer)
        {

            if (account.Balance >= amountTransfer)
                return true;
            else
                return false;
        }


        private void FreezeAccount()
        {
            m_frozen = true;
        }

        private void UnfreezeAccount()
        {
            m_frozen = false;
        }



    }
}
