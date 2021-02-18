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
        private LotteryProgram lotteryProgram;
        public IEnumerable<LotteryTicket> lotteryResults;

        public SettingsModel(LotteryProgram prog)
        {
            lotteryProgram = prog;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPostResetLottery()
        {
            lotteryProgram.ResetPeriod();
            return Page();
        }

        public IActionResult OnPostProcessDrawing()
        {
            if (lotteryProgram.ClosePeriodSales())
            {

            }
            if (lotteryProgram.p.DrawWinningTicket())
            {

            }
            try
            {
                lotteryProgram.p.ComputeWinners();
            }
            catch
            {

            }
            lotteryResults = lotteryProgram.p.ResultsByWinLevel();
            return Page();
        }
        public IActionResult OnPostLotteryResults()
        {
            lotteryResults = lotteryProgram.p.ResultsByWinLevel();
            return Page();
        }

        public IActionResult OnPostHistoricalStats()
        {

            return Page();
        }
    }
}
