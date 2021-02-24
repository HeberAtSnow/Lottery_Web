using ClassLib;
using System;
using TechTalk.SpecFlow;
using FluentAssertions;


namespace Tests
{
    [Binding]
    public class ClosePeriodSteps
    {
        private readonly ScenarioContext context;

        public ClosePeriodSteps(ScenarioContext context)
        {
            this.context = context;
        }

        [When(@"period is closed")]
        public void WhenPeriodIsClosed()
        {
            var ls = new LotteryStatistics();
            context.Add("connAnswer", ls.TestConn());
        }
        
        [Then(@"DB should answer version")]
        public void ThenDBShouldAnswerVersion()
        {
            context.Get<string>("connAnswer").Should().StartWith("PostgreSQL version:");
        }

        [Then(@"the database will have stats of winning tickets count = (.*) and losing tickets count = (.*)")]
        public void ThenTheDatabaseWillHaveStatsOfWinningTicketsCountAndLosingTicketsCount(int winCnt, int losCnt)
        {
            var ls = new LotteryStatistics();
            ls.WriteStatsToDB(context.Get<LotteryPeriod>("period") ).Should().Be(true); //junk evaluation TODO: better assert test
        }

    }
}
