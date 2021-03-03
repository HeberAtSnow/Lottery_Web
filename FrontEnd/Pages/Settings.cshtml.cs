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
        
        public IEnumerable<LotteryTicket> WinningTickets;
        public IEnumerable<TicketSale> PurchasedTickets;

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
        //draw winning numbers
        //current lottery results
        //all lottery statistics

        public void OnPostDrawWinningTickets()
        {
            //lp.ClosePeriodSales();
            lp.p.DrawWinningTicket();
            lp.p.ComputeWinners();
        }
        public void OnPostGetLotteryResults()
        {
            WinningTickets = lp.p.ResultsByWinLevel();
            //return RedirectToPage("./LotteryResults");
        }
    }
}
