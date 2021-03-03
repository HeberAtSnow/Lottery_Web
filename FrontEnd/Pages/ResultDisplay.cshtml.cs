using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class ResultDisplayModel : PageModel
    {
        LotteryProgram lp;
        public bool testBool;
        public IEnumerable<LotteryTicket> lotteryTickets;
        public ResultDisplayModel(LotteryProgram lotteryProgram)
        {
            lp = lotteryProgram;
        }

        public void OnGet()
        {

        }

        public void OnPostProcessDrawing()
        {
            testBool = lp.ClosePeriodSales();
            lp.p.DrawWinningTicket();
            lp.p.ComputeWinners();
            lotteryTickets = lp.p.ResultsByWinLevel();
        }
    }
}
