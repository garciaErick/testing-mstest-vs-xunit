using System;

namespace BankingSystem
{
    public class Program
    {
        public static void Main()
        {
            var ba = new BankAccount("Mr. Bryan Walton", 11.99);

            ba.Credit(5.77);
            PrintAccount(ba);
            ba.Debit(11.22);
            PrintAccount(ba);
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