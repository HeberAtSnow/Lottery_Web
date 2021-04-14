using ClassLib;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Tests
{
    [Binding]
    public sealed class PeriodSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext context;

        public PeriodSteps(ScenarioContext scenarioContext)
        {
            context = scenarioContext;
        }

        [Given(@"a new period")]
        public void GivenANewPeriod()
        {
            var program = new LotteryProgram();
            context.Add("period", program.Period);
            context.Add("vendor", program.Vendor);
            context.Add("program", program);
        }

        [Given(@"a ticket was sold with the numbers (.*)")]
        public void GivenATicketWasSoldWithTheNumbers(string numbers)
        {
            var nums = numbers.Split(',').Select(n => int.Parse(n)).ToArray();
            var vendor = context.Get<LotteryVendor>("vendor");
            var lt = vendor.SellTicket("bob", nums);
            context.Add("ticket", lt);
        }

        [When(@"the winning ticket is (.*)")]
        public void WhenTheWinningTicketIs(string numbers)
        {
            var nums = numbers.Split(',').Select(n => int.Parse(n)).ToArray();
            var t = new LotteryTicket("WinningTicket", nums);
            var period = context.Get<LotteryPeriod>("period");
            period.WinningTicket = t;
        }

        [Then(@"the sold ticket wins at level (.*) and prize amt is (.*)")]
        public void ThenTheSoldTicketWinsAtLevelY(int level, string winamt)
        {
            var lt = context.Get<LotteryTicket>("ticket");
            var period = context.Get<LotteryPeriod>("period");
            period.CheckWinningTicket(lt);
            lt.winLevel.Should().Be(level);
            lt.winAmtDollars.Should().Be(decimal.Parse(winamt));

        }


    }
}
