using ClassLib;
using FluentAssertions;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace Tests
{
    [Binding]
    public class LotteryComputeStatsSteps
    {
        private readonly ScenarioContext context;

        public LotteryComputeStatsSteps(ScenarioContext context)
        {
            this.context = context;
        }


        [Given(@"a ticket was sold to (.*) with the numbers (.*)")]
        public void GivenATicketWasSoldToPlayerNameWithTheNumbersNumbers(string playerName, string numbers )
        {
            var vendor = context.Get<LotteryVendor>("vendor");
            var nums = numbers.Split(',').Select(n => int.Parse(n)).ToArray();
            vendor.SellTicket(playerName, nums);
        }
        
        [When(@"statistics are computed")]
        public void WhenStatisticsAreComputed()
        {
            var p = context.Get<LotteryPeriod>("period");
            p.ComputeWinners();

        }

        [Then(@"the count of winning tickets should be (.*)")]
        public void ThenTheCountOfWinningTicketsShouldBe(int p0)
        {
            var p = context.Get<LotteryPeriod>("period");
            p.winningTickets.Count().Should().Be(p0);
        }

        [Then(@"the count of losing tickets should be (.*)")]
        public void ThenTheCountOfLosingTicketsShouldBe(int p0)
        {
            var p = context.Get<LotteryPeriod>("period");
            p.loosingTickets.Count().Should().Be(p0);
        }
        
        [Then(@"(.*) should have (.*) winning tickets")]
        public void ThenPlayerNameShouldHaveNWinningTickets(string playerName, int qty)
        {
            var p = context.Get<LotteryPeriod>("period");
            //Well, here's a problem
            //I have a stack (p.winningTickets) that I need to query (preferrably with Linq)
            //problem is, a stack data structure can only be read from without peek/pop
            //this means I will not be able to traverse it without destroying it.
            ///grrrr.
            ///Option 1:  Goto the period.ComputeWinners() and change the winningTicket and loosingTicket to lists
            ///Option 2:  Keep crying
        }

        [Then(@"(.*) should have (.*) losing tickets")]
        public void ThenPlayerNameShouldHaveNLosingTickets(string playerName, int qty)
        {
            var p = context.Get<LotteryPeriod>("period");
        }
    }
}
