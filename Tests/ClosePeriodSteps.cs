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
            var pID = ls.WriteStatsToDB(context.Get<LotteryPeriod>("period"));
            
            int winTcntDB; //winning Ticket Count From DB
            //TODO - read counts from DB
            winTcntDB = ls.DBWinningTicketCountInPeriod(pID);
            winTcntDB.Should().Equals(winCnt); //junk evaluation TODO: better assert test
            
            int loseTcntDB; //loosing ticket count from DB
            //TODO - read counts from DB
            loseTcntDB = ls.DBLoosingTicketCountInPeriod(pID);
            loseTcntDB.Should().Equals(losCnt); //junk evaluation TODO: better assert test

            
        }

    }
}
