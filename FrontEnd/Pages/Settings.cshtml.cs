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
        private readonly ILogger<SettingsModel> _logger;
        LotteryProgram lp;
        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period

        public IEnumerable<LotteryTicket> WinningTickets;
        public IEnumerable<TicketSale> PurchasedTickets;

        public SettingsModel(LotteryProgram lotteryProgram, ILogger<SettingsModel> logger)
        {
            lp = lotteryProgram;
            _logger = logger;
        }
        public void OnGet()
        {
            _logger.LogInformation("Admin page was loaded");
        }

        public void OnPostResetLottery()
        {
            lp.ResetPeriod();
            _logger.LogInformation("Lottery period was reset");
        }
        //draw winning numbers
        //current lottery results
        //all lottery statistics

        public void OnPostDrawWinningTickets()
        {
            //lp.ClosePeriodSales();
            lp.p.DrawWinningTicket();
            lp.p.ComputeWinners();
            _logger.LogInformation("Lottery Winning Ticket was drawn");
        }
        public void OnPostGetLotteryResults()
        {
            WinningTickets = lp.p.ResultsByWinLevel();
            //return RedirectToPage("./LotteryResults");
            _logger.LogInformation("Lottery results button was pushed");
        }
        public void OnPostGetLotteryStats()
        {
            _logger.LogInformation("Lottery stats button was pushed");
            //WinningTickets = lp.p.ResultsByWinLevel();
            //return RedirectToPage("./LotteryStats");
        }
    }
}
