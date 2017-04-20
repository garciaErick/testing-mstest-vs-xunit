using System;

namespace BankingSystem
{
    public class Program
    {
        public static void Main()
        {
            var ba = new BankAccount("Walter White", 400000);
            var ba2 = new BankAccount("Saul Goodman", 50);
            Console.WriteLine("Original Bank Accounts");
            PrintAccount(ba);
            PrintAccount(ba2);
            ba.WithDraw(10000);
            Console.WriteLine("After Withdrawing 10000");
            PrintAccount(ba);
            ba.Deposit(5000);
            Console.WriteLine("After Depositing 5000");
            PrintAccount(ba);
            ba.Transfer(ba2, 100000);
            Console.WriteLine("After Transfering 10000 to Saul Goodman from Walter White");
            PrintAccount(ba);
            PrintAccount(ba2);
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();

        }

        public static void PrintAccount(BankAccount ba)
        {
            Console.WriteLine("Account name: " + ba.CustomerName);
            Console.WriteLine("Balance: " + ba.Balance);
            Console.WriteLine();

        }
    }
}