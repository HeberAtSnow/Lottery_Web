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
        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        public SettingsModel(LotteryProgram lotteryProgram, ILogger<SettingsModel> logger )
        {
            lp = lotteryProgram;
            this.logger = logger;
        }
        public void OnGet()
        {
            logger.LogInformation("went to Administration page {time}");
        }

        public void OnPostResetLottery()
        {
            logger.LogInformation("clicked ResetLottery");
            try { 
               var x=lp.ResetPeriod();
                if (x)
                    msg = "Reset the period";
            }
            catch
            {
                error = "cant reset period when salesa are ok";
                logger.LogError(error);
            }
            
        }
        public IActionResult OnPostDrawLottery()
        {
            logger.LogInformation("clicked DrawLottery");
            lp.ClosePeriodSales();
            lp.p.DrawWinningTicket();
            lp.p.ComputeWinners();
            return Page();
        }
        public IActionResult OnPostResultsLottery()
        {
            var lotteryResults = lp.p.ResultsByWinLevel();
            return Page();
        }
        public IActionResult OnPostAllStats()
        {
            try { 
            stats = ls.DBStatsAllPeriods();
            }
            catch
            {
                error = "failed to get history";
            }
            return Page();
        }
        public IActionResult OnPostBackToIndex()
        {
            return RedirectToPage("./Index");
        }
    }
}
