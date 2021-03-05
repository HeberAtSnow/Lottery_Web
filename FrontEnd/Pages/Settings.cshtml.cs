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
            lp.ResetPeriod();
        }

        public void OnPostProcessDrawing()
        {
            lp.ClosePeriodSales();
            lp.p.DrawWinningTicket();
            lp.p.ComputeWinners();
        }
        public IActionResult OnPostLotteryResults()
        {
            var results = lp.p.ResultsByWinLevel();
            return RedirectToPage("./LotteryResults", results);
        }

        public IActionResult OnPostHistoricalStats()
        {
            LotteryStatistics ls = new LotteryStatistics();
            var results = ls.DBStatsAllPeriods();
            return RedirectToPage("./HistoricalStats", results);
        }
        //draw winning numbers
        //current lottery results
        //all lottery statistics
    }
}
