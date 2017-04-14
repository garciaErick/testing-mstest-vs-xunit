using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console_BlackJack_cs;

namespace MsTestDemo
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()   
        {
            Blackjack bjGame = new Blackjack();
            bjGame.beginGame();
            int noOfCards = bjGame.playerCards.Length;
            bjGame.Hit();
            int noOfCardsAfterHit = bjGame.playerCards.Length;
            Assert.AreNotEqual(noOfCards, noOfCardsAfterHit );
            Environment.Exit(0);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.AreEqual(2, 1 + 1);
        }

        [TestMethod]
        public void TestMethod3()
        {
            String x = "hello";
            Blackjack bj = new Blackjack();
            Assert.AreNotEqual(x, bj.Deal());
        }
    }
}
