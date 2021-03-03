using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{

    public class SettingsModel : PageModel
    {
        LotteryProgram lp;
        LotteryStatistics ls = new LotteryStatistics();
        public IEnumerable<TicketSale> stats;
        public string error;
        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        public SettingsModel(LotteryProgram lotteryProgram)
        {
            lp = lotteryProgram;
        }
        public void OnGet()
        {
        }

        public void OnPostResetLottery()
        {
            try { 
            lp.ResetPeriod();
            }
            catch
            {
                error = "cant reset period when salesa are ok";
            }
        }
        public IActionResult OnPostDrawLottery()
        {
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
