using ClassLib;
using FluentAssertions;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace Tests
{
    [Binding]
    public class TicketCreationSteps
    {
        private readonly ScenarioContext context;

        public TicketCreationSteps(ScenarioContext context)
        {
            this.context = context;
        }
        [Given(@"username is (.*)")]
        public void GivenUsername(string name)
        {
            context.Add("playerName", name);

        }
        
        [Given(@"my six numbers are (.*)")]
        public void GivenMySixNumbersAre(string numbers)
        {
            var nums = numbers.Split(',').Select(n => int.Parse(n)).ToArray();
            context.Add("sixNumbers", nums);
        }

        [When(@"I create a ticket")]
        public void WhenICreateATicket()
        {
            var nums = context.Get<int[]>("sixNumbers");
            string playerName;
            try
            {
                playerName = context.Get<string>("playerName");
            }
            catch
            {
                playerName = null;
            }
            try
            {
                var t = new LotteryTicket(playerName, nums);
                context.Add("ticket", t);
                context.Add("msg", true);
            }
            catch
            {
                context.Add("msg", false);

            }

        }
        
        [Then(@"the ticket (.*) valid")]
        public void ThenTheTicketIsValid(string test)
        {
            var expectedResult = test=="is";
            var result = context.Get<bool>("msg");
            result.Should().Be(expectedResult);
        }
    }
}
