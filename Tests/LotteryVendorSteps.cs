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
    public sealed class LotteryVendorSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext context;

        public LotteryVendorSteps(ScenarioContext scenarioContext)
        {
            context = scenarioContext;
        }



        [When(@"vendor sells (.*) quickPick tickets")]
        public void WhenVendorSellsNQuickPickTickets(int qty)
        {
            var vendor = context.Get<LotteryVendor>("vendor");
            vendor.SellQuickTickets("bob", qty);
        }

        [Then(@"the period should have another (.*) tickets added to stack")]
        public void ThenThePeriodShouldHaveAnotherXTicketsAddedToStack(int qty)
        {
            var p = context.Get<LotteryPeriod>("period");
            p.soldTickets.Count.Should().Be(qty);
        }

        [When(@"vendor sells (.*) custom ticket")]
        public void WhenVendorSellsCustomTicket(int qty)
        {
            var vendor = context.Get<LotteryVendor>("vendor");
            vendor.SellTicket("bob");
        }

        [When(@"Three background threads sell (.*) tickets each")]
        public void WhenThreeBackgroundThreadsSellTicketsEach(int numToSell)
        {
            var vendor = context.Get<LotteryVendor>("vendor");
            vendor.StartSimulatedTicketSales(numToSell);
        }



    }
}
