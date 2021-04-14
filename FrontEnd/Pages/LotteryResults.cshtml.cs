using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Pages
{
    public class LotteryResultsModel : PageModel
    {
        LotteryProgram lp { get; }
        public IEnumerable<LotteryTicket> results { get; set; }

        private readonly ILogger<LotteryResultsModel> _logger;

        
        public IEnumerable<LotteryTicket> WinLotteryTickets;
        public IEnumerable<TicketSale> CurrPeriodWinTickets;

        public IEnumerable<TicketSale> ticketSales;
        public IEnumerable<(int periodid, DateTime started)> LotteryPeriods;

        public LotteryResultsModel(LotteryProgram lotteryProgram, LotteryStatistics lotteryStatistics, ILogger<LotteryResultsModel> logger)
        {
        
            lp = lotteryProgram;
            _logger = logger;
        }
        public void OnPost()
        {
            _logger.LogInformation("LotteryResults page was called");
            LotteryResults();
        }

        public void LotteryResults()
        {

            _logger.LogInformation("clicked Lottery Results");


            WinLotteryTickets = lp.p.ResultsByWinLevel();

           

            if (WinLotteryTickets == null)
            {
                _logger.LogWarning("There are no winning Lotteries");
            }
            else
            {
                _logger.LogInformation("Winner Lottery count: {count}", WinLotteryTickets.Count());
            }

           /* startTime = DateTime.Now;

            LotteryPeriods = lotteryStatistics.DBPeriodsInHistory();
            var list = LotteryPeriods.ToList();

            CurrPeriodWinTickets = lotteryStatistics.DBStatsOnePeriod(list.Last().periodid);

            _logger.LogInformation(" time to complete results of the Current Period Winning Lottery Tickets took {eleapsed}", DateTime.Now - startTime);

            if (CurrPeriodWinTickets == null)
            {
                _logger.LogWarning("There are no winning winning lotteries in current period");
            }
            else
            {
                _logger.LogInformation("Winner Lottery count is: {count} in period with id {periodid}", CurrPeriodWinTickets.Count(), list.Last().periodid);
            }*/

        }
    }
}
