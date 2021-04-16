using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly ILogger<IndexModel> _logger;

        public SettingsModel(LotteryProgram lotteryProgram, ILogger<IndexModel> logger)
        {
            lp = lotteryProgram;
            _logger = logger;
        }
        public void OnGet()
        {
            _logger.LogDebug("{prefix}: Setting page successfully loaded on {dow}, {time}", LogPrefix.WebTraffic, DateTime.Now.ToString("ddd"), DateTime.Now);

        }

        public IActionResult OnPostResetLottery()
        {

            _logger.LogDebug("{prefix}: The reset lottery button has been clicked on {dow}, {time}",
                LogPrefix.ButtonClick, DateTime.Now.ToString("ddd"), DateTime.Now);

            try
            {
                lp.ResetPeriod();
            }
            catch (Exception ex)
            {
                _logger.LogError("{prefix}: Lottery failed to reset on {dow}, {time} with the following exception: \n{ex}",
                    LogPrefix.Functionality, DateTime.Now.ToString("ddd"), DateTime.Now, ex);
                return Page();
            }
            _logger.LogInformation("{prefix}: Lottery successfully reset on {dow}, {time}", LogPrefix.Functionality, DateTime.Now.ToString("ddd"), DateTime.Now);
            return Page();
        }
        //draw winning numbers
        public IActionResult OnPostDrawWinningNumbers()
        {
            _logger.LogDebug("{prefix}: Clicked draw winning numbers, started drawing numbers on {dow}, {time}.",
                LogPrefix.ButtonClick, DateTime.Now.ToString("ddd"), DateTime.Now);

            try
            {
                lp.ClosePeriodSales();
                lp.p.DrawWinningTicket();
                lp.p.ComputeWinners();

                _logger.LogInformation("{prefix}: Winners for current period were successfully drawn on {dow}, time.",
                    LogPrefix.Functionality, DateTime.Now.ToString("ddd"));

                var totalRevenue = GetRevenueForThisPeriod();
                _logger.LogInformation("{prefix}: The total amount of money collected in this loggery period is ${total}", LogPrefix.Business, totalRevenue);

                var totalprofit = getProfitForThisPeriod();
                _logger.LogInformation("{prefix}: the total profit for this lottery period is ${profit}", LogPrefix.Business, totalprofit);

                var totalCost = totalRevenue - totalprofit;
                _logger.LogInformation("{prefix}: The total amount of winnings for this lottery period is ${cost}", LogPrefix.Business, totalCost);
            }
            catch (Exception ex)
            {
                _logger.LogError("{prefix}: Failed to draw winning numbers on {dow}, {time} with the following exception \n{ex}",
                    LogPrefix.Functionality, DateTime.Now.ToString("ddd"), DateTime.Now, ex);
                return Page();
            }

            _logger.LogDebug("{prefix}: Selecting winning lottery numbers was successful on {dow}, {time}",
                LogPrefix.Functionality, DateTime.Now.ToString("ddd"), DateTime.Now);

            return Page();

        }
        //current lottery results
        public IActionResult OnPostGoToResults()
        {

            _logger.LogDebug("{prefix}: Clicked on the lottery results button on {dow}, {time}.",
                LogPrefix.ButtonClick, DateTime.Now.ToString("ddd"), DateTime.Now);

            return RedirectToPage("./LotteryResults");
        }
        //all lottery statistics
        public IActionResult OnPostLotteryStatistics()
        {
            _logger.LogDebug("{prefix}: Clicked on the lottery statistics button on {dow}, {time}.",
                LogPrefix.ButtonClick, DateTime.Now.ToString("ddd"), DateTime.Now);
            return Page();
        }


        private decimal GetRevenueForThisPeriod() {
            var totalTickets = lp.p.ResultsByWinLevel().Count();
            return totalTickets * 2;
        }
        private decimal getProfitForThisPeriod()
        {
            decimal revenue = GetRevenueForThisPeriod();
            decimal cost = lp.p.winningTicketsL.Sum(t => t.winAmtDollars);
            return (revenue - cost);
        }
    }
}
