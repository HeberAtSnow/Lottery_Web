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
        private readonly LotteryProgram LotteryProgram;
        private readonly ILogger<SettingsModel> logger;

        public SettingsModel(LotteryProgram lotteryProgram, ILogger<SettingsModel> logger)
        {
            LotteryProgram = lotteryProgram;
            this.logger = logger;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostResetLottery()
        {
            try
            {
                LotteryProgram.ResetPeriod();

                logger.LogInformation("[{prefix}]: Lottery period has been successfully restarted.",
                    LogPrefix.AdminFunc);
            }
            catch (Exception ex)
            {
                logger.LogError("[{prefix}]: Failed to restart current lottery period. Reason: {ex}.",
                    LogPrefix.AdminFunc, ex);

                return RedirectToPage();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDrawWinners()
        {
            try
            {
                LotteryProgram.ClosePeriodSales();
                LotteryProgram.Period.DrawWinningTicket();
                LotteryProgram.Period.ComputeWinners();

                logger.LogInformation("[{prefix}]: Winners for current period were successfully drawn.",
                    LogPrefix.AdminFunc);

                var totalRev = GetRevenueForThisPeriod();
                logger.LogInformation("[{prefix}]: A total of ${amount} was collected during this lottery period.",
                    LogPrefix.Business, totalRev);

                var totalProfit = GetProfitForThisPeriod();
                logger.LogInformation("[{prefix}]: The total profit for this lottery period was ${amount}",
                    LogPrefix.Business, totalProfit);

                var totalLoss = totalRev - totalProfit;
                logger.LogInformation("[{prefix}]: The total amount paid to winners of this period was ${amount}",
                    LogPrefix.Business, totalLoss);
            }
            catch (Exception ex)
            {
                logger.LogError("[{prefix}]: Failed to draw winners for current period. Reason: {ex}.",
                    LogPrefix.AdminFunc, ex);

                return RedirectToPage("./Error");
            }

            return RedirectToPage();
        }

        private decimal GetRevenueForThisPeriod()
        {
            var totalTickets = LotteryProgram.Period.ResultsByWinLevel().Count();
            return totalTickets * 2;
        }

        private decimal GetProfitForThisPeriod()
        {
            var rev = GetRevenueForThisPeriod();
            var loss = LotteryProgram.Period.winningTicketsL.Sum(t => t.winAmtDollars);
            return rev - loss;
        }
    }
}
