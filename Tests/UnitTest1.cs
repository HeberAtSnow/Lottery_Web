using NUnit.Framework;
using ClassLib;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Ticket_Constructor_NameIsGiven()
        {
            LotteryTicket l = new LotteryTicket("Bob");
            Assert.AreEqual(l.Player, "Bob");
        }
        [Test]
        public void Ticket_Constructor_NoNameGiven()
        {
            LotteryTicket l = new LotteryTicket();
            Assert.AreEqual(l.Player, "Player Name Anonymous");
        }


        [TestCase("bob", new[] { 1, 2, 3, 4, 5, 6 }, ExpectedResult = true)]
        [TestCase("bob", new[] { 1, 2, 3, 4, 4, 6 }, ExpectedResult = false)]
        [TestCase("bob", new[] { 1, 2, 3, 4, 70, 6 }, ExpectedResult = false, Description = "ball <=69")]
        [TestCase("bob", new[] { 1, 2, 3, 4, 5, 0 }, ExpectedResult = false, Description = "powerball limit >=1")]
        [TestCase("bob", new[] { 1, 2, 3, 4, 5, 27 }, ExpectedResult = false, Description = "powerball limit <=26")]
        [TestCase("bob", new[] { 0, 2, 3, 4, 4, 6 }, ExpectedResult = false, Description = "ball >=1")]
        [TestCase("bob", new[] { 1, 2, 3, 4, 4, 6 }, ExpectedResult = false)]
        public bool test_tickets(string name, int[] sixnumbers)
        {
            try
            {
                var t = new LotteryTicket(name, sixnumbers);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}