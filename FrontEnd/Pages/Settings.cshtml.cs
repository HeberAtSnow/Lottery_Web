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
        public bool appear = false;
        public SettingsModel(LotteryProgram lotteryProgram)
        {
            lp = lotteryProgram;
        }
        public void OnGet()
        {
        }
    
        public IActionResult OnPostResetLottery()
        {
            lp.ResetPeriod();
            return Page();
        }
        //draw winning numbers
        public IActionResult OnPostDrawWinningNumbers()
        {
            lp.ClosePeriodSales();
            lp.p.DrawWinningTicket();
            lp.p.ComputeWinners();
            appear = true;
            return Page();
            
        }
        //current lottery results
        public IActionResult OnPostGoToResults()
        {
            return RedirectToPage("./LotteryResults");
        }
        //all lottery statistics
        public IActionResult OnPostLotteryStatistics() { 
            return Page();
        }
    }
}
