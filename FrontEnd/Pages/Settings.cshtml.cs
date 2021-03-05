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
        private readonly ILogger<SettingsModel> logger;
        LotteryStatistics ls = new LotteryStatistics();
        public IEnumerable<TicketSale> stats;
        public string error;
        public string msg = "";
        public DateTime startTime= new DateTime();
        public DateTime endTime = new DateTime();
        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        public SettingsModel(LotteryProgram lotteryProgram, ILogger<SettingsModel> logger )
        {
            lp = lotteryProgram;
            this.logger = logger;
        }
        public void OnGet()
        {
            var time = DateTime.Now;
            logger.LogInformation("went to Administration page {time}", time);
        }

        public void OnPostResetLottery()
        {
            logger.LogInformation("clicked ResetLottery");
            try { 
               var x=lp.ResetPeriod();
                if (x)
                    msg = "Reset the period";
                logger.LogInformation(msg);
            }
            catch
            {
                error = "cant reset period when salesa are ok";
                logger.LogError(error);
            }
            
        }
        public IActionResult OnPostDrawLottery()
        {
            startTime = DateTime.Now;
            logger.LogInformation("clicked DrawLottery");
            lp.ClosePeriodSales();
            lp.p.DrawWinningTicket();
            lp.p.ComputeWinners();
            endTime = DateTime.Now;
            var time = endTime - startTime;
            logger.LogInformation("DrawLottery took {time} to process", time);
            return Page();
        }
        public IActionResult OnPostResultsLottery()
        {
            logger.LogInformation("clicked Result lottery");
            var startTime = DateTime.Now;
            var lotteryResults = lp.p.ResultsByWinLevel();
            var endTime = DateTime.Now;
            var eleapsed = endTime - startTime;
            logger.LogInformation(" time to complete results of the lotter {eleapsed}", eleapsed);
            if (lotteryResults == null)
            {
                logger.LogWarning("Lottery results is null");
            }
            else
            {
                logger.LogInformation("lottery count = {count}", lotteryResults.Count());
            }
            return Page();
        }
        public IActionResult OnPostAllStats()
        {
            logger.LogInformation("Clicke on get all stats");
            try {
                var startTime = DateTime.Now;
            stats = ls.DBStatsAllPeriods();
                var endTime = DateTime.Now;
                var elapsed = endTime - startTime;
                logger.LogInformation("To get the stats took {elapsed}", elapsed);
                if (stats == null)
                {
                    logger.LogWarning("Stats returned null");
                }
                else
                {
                    logger.LogInformation("stats returned {count} stats", stats.Count());
                }
            }
            catch
            {
                error = "failed to get history";
                logger.LogWarning(error);
            }
            return Page();
        }
        public IActionResult OnPostBackToIndex()
        {
            logger.LogInformation("Returning to index");
            return RedirectToPage("./Index");
        }
    }
}
