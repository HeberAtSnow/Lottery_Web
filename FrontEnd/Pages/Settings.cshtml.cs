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
        LotteryStatistics lotteryStatistics = new LotteryStatistics();
        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        public SettingsModel(LotteryProgram lotteryProgram)
        {
            lp = lotteryProgram;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPostResetLottery()
        {
            try
            {
                lp.ResetPeriod();
                return Page();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Page();
            }

        }
        public IActionResult OnPostDrawWinningNumbers()
        {
            try
            {
                lp.ClosePeriodSales();
                lp.p.DrawWinningTicket();
                lp.p.ComputeWinners();
                return RedirectToPage("./Winners",lp.p.winningTicketsL);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Page();
            }
        }

        public IActionResult OnPostGetStatistics()
        {
            try
            {
                return RedirectToPage("./Stats", lotteryStatistics.DBStatsAllPeriods());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Page();
            }

        }
        //draw winning numbers
        //current lottery results
        //all lottery statistics
    }
}
