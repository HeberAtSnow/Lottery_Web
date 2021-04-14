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
            var prg = context.Get<LotteryProgram>("program");
            prg.ClosePeriodSales();
            var p = context.Get<LotteryPeriod>("period");
            p.ComputeWinners();

        }

        [Then(@"the count of winning tickets should be (.*)")]
        public void ThenTheCountOfWinningTicketsShouldBe(int p0)
        {
            var p = context.Get<LotteryPeriod>("period");
            p.winningTicketsL.Count().Should().Be(p0);
        }

        [Then(@"the count of losing tickets should be (.*)")]
        public void ThenTheCountOfLosingTicketsShouldBe(int p0)
        {
            var p = context.Get<LotteryPeriod>("period");
            p.losingTicketsL.Count().Should().Be(p0);
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



        [Then(@"the results for (.*) should be")]
        public void ThenTheResultsForBobShouldBe(string inPlayerName, Table table)
        {
            //WORK NEEDED
            //TODO:  I don't like the constructor making a dirty ticket record.
            //       won't have grading, etc
            var p = context.Get<LotteryPeriod>("period");
            var actualTickets = p.ResultsByPlayer(inPlayerName);

            var expectedTickets = new List<LotteryTicket>();
            foreach ( var row in table.Rows)
            {
                var ticket = new LotteryTicket();
                ticket.balls[0] = int.Parse(row["b0"]);
                ticket.balls[1] = int.Parse(row["b1"]);
                ticket.balls[2] = int.Parse(row["b2"]);
                ticket.balls[3] = int.Parse(row["b3"]);
                ticket.balls[4] = int.Parse(row["b4"]);
                ticket.powerBall = int.Parse(row["pb"]);
                ticket.Type = row["type"];
                ticket.isGraded = true;
                ticket.Player = inPlayerName;
                ticket.winLevel = int.Parse(row["winLevel"]);
                ticket.winAmtDollars = int.Parse(row["winAmt"]);
                expectedTickets.Add(ticket);
            }
            actualTickets.Should().BeEquivalentTo(expectedTickets);
        }

    }
}
