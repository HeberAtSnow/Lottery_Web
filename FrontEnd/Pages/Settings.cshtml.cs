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
   
        private readonly LotteryStatistics lotteryStatistics;
        public IEnumerable<LotteryTicket> WinLotteryTickets;
        public IEnumerable<TicketSale> ticketSales;

        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        public SettingsModel(LotteryProgram lotteryProgram, LotteryStatistics lotteryStatistics)
        {
            lp = lotteryProgram;
            this.lotteryStatistics = lotteryStatistics;
        }
        public void OnGet()
        {
        }

        public void OnPostResetLottery()
        {
            lp.ResetPeriod();
        }
        //draw winning numbers
        public void OnPostDrawWinningTickets()
        {
            lp.ClosePeriodSales();
            lp.p.DrawWinningTicket();
            lp.p.ComputeWinners();
        }
        //current lottery results
        public void OnPostLotteryResults()
        {
            WinLotteryTickets = lp.p.ResultsByWinLevel();
        }

        //all lottery statistics
        public void OnPostLotteryStats()
        {
            lotteryStatistics.DBPeriodsInHistory();

            ticketSales = lotteryStatistics.DBStatsAllPeriods();

        }
    }
}
