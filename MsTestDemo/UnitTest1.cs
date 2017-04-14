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
            int noOfCards = bjGame.playerCards.Length;
            bjGame.Hit();
            int noOfCardsAfterHit = bjGame.playerCards.Length;
            Assert.AreNotEqual(noOfCards, noOfCardsAfterHit );
        }
    }
}
