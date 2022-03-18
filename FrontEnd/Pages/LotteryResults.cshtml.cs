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


        public IEnumerable<LotteryTicket> WinLotteryTickets;
        public IEnumerable<TicketSale> CurrPeriodWinTickets;
        private readonly LotteryStatistics lotteryStatistics;
        private readonly ILogger<LotteryResultsModel> logger;
        public IEnumerable<TicketSale> ticketSales;
        public IEnumerable<(int periodid, DateTime started)> LotteryPeriods;

        public LotteryResultsModel(LotteryProgram lotteryProgram, LotteryStatistics _lotteryStatistics, ILogger<LotteryResultsModel> logger)
        {
            lotteryStatistics = _lotteryStatistics;
            this.logger = logger;
            lp = lotteryProgram;
        }
        public void OnGet()
        {
            LotteryResults();
        }

        public IActionResult LotteryResults()
        {
            WinLotteryTickets = lp.Period.ResultsByWinLevel();
            LotteryPeriods = lotteryStatistics.DBPeriodsInHistory();
            var list = LotteryPeriods.ToList();

            CurrPeriodWinTickets = lotteryStatistics.DBStatsOnePeriod(list.Last().periodid);

            try
            {
                results = lp.Period.ResultsByWinLevel();
                logger.LogDebug("[{prefix}]: Successfully retrieved results for the current lottery period",
                    LogPrefix.Stats);

                return Page();
            }
            catch (Exception ex)
            {
                logger.LogWarning("[{prefix}]: Failed to retrieve the results for the current lottery period. Reason: {ex}",
                    LogPrefix.Stats, ex);

                return RedirectToPage("./Error");
            }

        }
        /*public IActionResult OnGet()
        {
            try
            {
                results = LotteryProgram.Period.ResultsByWinLevel();
                logger.LogDebug("[{prefix}]: Successfully retrieved results for the current lottery period",
                    LogPrefix.Stats);

                return Page();
            }
            catch (Exception ex)
            {
                logger.LogWarning("[{prefix}]: Failed to retrieve the results for the current lottery period. Reason: {ex}",
                    LogPrefix.Stats, ex);

                return RedirectToPage("./Error");
            }
        }*/
    }
}
