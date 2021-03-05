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

    public class SettingsModel : PageModel
    {
        LotteryProgram lp;

        public string ResetErrorMessage;
        public string ResetSuccessMessage = "";

        private readonly LotteryStatistics lotteryStatistics;
        public IEnumerable<LotteryTicket> WinLotteryTickets;
        public IEnumerable<TicketSale> CurrPeriodWinTickets;

        public IEnumerable<TicketSale> ticketSales;
        public IEnumerable<(int periodid, DateTime started)> LotteryPeriods;

        public ILogger<SettingsModel> logger { get; }

        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        public SettingsModel(LotteryProgram lotteryProgram, LotteryStatistics lotteryStatistics, ILogger<SettingsModel> logger)
        {
            lp = lotteryProgram;
            this.lotteryStatistics = lotteryStatistics;
            this.logger = logger;
        }
        public void OnGet()
        {

            logger.LogInformation("Somebody went to Administration page {time}", DateTime.Now);
        }

        public void OnPostResetLottery()
        {
            logger.LogInformation("Somebody clicked on ResetLottery {time}", DateTime.Now);

            try
            {
                if (lp.ResetPeriod())
                {
                    ResetSuccessMessage = "Successfully resetted the Lottery";
                    logger.LogInformation(ResetSuccessMessage);
                }
            }
            catch
            {
                ResetErrorMessage = "cant reset period when sales are not closed";
                logger.LogError(ResetErrorMessage);
            }


        }
        //draw winning numbers
        public void OnPostDrawWinningTickets()
        {
            var startTime = DateTime.Now;

            logger.LogInformation("Somebody clicked DrawLottery");

            lp.ClosePeriodSales();

            logger.LogInformation("ClosePeriodSales took {time} to process", DateTime.Now - startTime);

            lp.p.DrawWinningTicket();
            logger.LogInformation("DrawWinningTickets took {time} to process", DateTime.Now - startTime);

            lp.p.ComputeWinners();
            logger.LogInformation("ComputeWinners took {time} to process", DateTime.Now - startTime);

            logger.LogInformation("To finish DrawWinningTickets in total took {time} to process", DateTime.Now - startTime);
        }

        //current lottery results
        public void OnPostLotteryResults()
        {

            logger.LogInformation("clicked Lottery Results");

            var startTime = DateTime.Now;

            WinLotteryTickets = lp.p.ResultsByWinLevel();

            logger.LogInformation(" time to complete results of the WinningLottery Tickets took {eleapsed}", DateTime.Now - startTime);

            if (WinLotteryTickets == null)
            {
                logger.LogWarning("There are no winning Lotteries");
            }
            else
            {
                logger.LogInformation("Winner Lottery count: {count}", WinLotteryTickets.Count());
            }

            startTime = DateTime.Now;

            LotteryPeriods = lotteryStatistics.DBPeriodsInHistory();
            var list = LotteryPeriods.ToList();

            CurrPeriodWinTickets = lotteryStatistics.DBStatsOnePeriod(list.Last().periodid);

            logger.LogInformation(" time to complete results of the Current Period Winning Lottery Tickets took {eleapsed}", DateTime.Now - startTime);

            if (CurrPeriodWinTickets == null)
            {
                logger.LogWarning("There are no winning winning lotteries in current period");
            }
            else
            {
                logger.LogInformation("Winner Lottery count is: {count} in period with id {periodid}", CurrPeriodWinTickets.Count(), list.Last().periodid);
            }

        }

        //all lottery statistics
        public void OnPostLotteryStats()
        {
            logger.LogInformation("Clicked button to get all lottery results");
            try
            {
                var startTime = DateTime.Now;
                ticketSales = lotteryStatistics.DBStatsAllPeriods();
            
                logger.LogInformation("To get the all stats in all periods took {elapsed}", DateTime.Now - startTime);

                if (ticketSales == null)
                {
                    logger.LogWarning("There is not ticket sale period history");
                }
                else
                {
                    logger.LogInformation("website returned {count} ticker sale periods", ticketSales.Count());
                }
            }
            catch
            {
                ResetErrorMessage = "failed to get all periods history";
                logger.LogWarning(ResetErrorMessage);
            }

            

        }
    }
}
