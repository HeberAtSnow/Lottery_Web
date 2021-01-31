using ClassLib;
using FluentAssertions;
using System;
using System.Collections.Generic;
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


        [Given(@"the winning ticket is (.*)")]
        public void GivenTheWinningTicketIsX(string numbers)
        {
            var nums = numbers.Split(',').Select(n => int.Parse(n)).ToArray();
            var t = new LotteryTicket("WinningTicket", nums);
            var period = context.Get<LotteryPeriod>("period");
            period.WinningTicket = t;
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
            p.losingTickets.Count().Should().Be(p0);
        }
        
        [Then(@"(.*) should have (.*) winning tickets")]
        public void ThenPlayerNameShouldHaveNWinningTickets(string inPlayerName, int qty)
        {
            var p = context.Get<LotteryPeriod>("period");
            var filtered = p.winningTicketsL.Where(t => t.Player == inPlayerName);
            filtered.Count().Should().Be(qty);

        }

        [Then(@"(.*) should have (.*) losing tickets")]
        public void ThenPlayerNameShouldHaveNLosingTickets(string inPlayerName, int qty)
        {
            var p = context.Get<LotteryPeriod>("period");
            var filtered = p.losingTicketsL.Where(t=> t.Player == inPlayerName );
            filtered.Count().Should().Be(qty);

        }
    }
}
