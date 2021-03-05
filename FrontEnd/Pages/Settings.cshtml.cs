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
        private readonly ILogger logger;

        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        public SettingsModel(LotteryProgram lotteryProgram, ILogger logger)
        {
            lp = lotteryProgram;
            this.logger = logger;
        }
        public void OnGet()
        {
            logger.LogDebug("Settings page was loaded");
        }

        public IActionResult OnPostResetLottery()
        {
            var stopwatch = new Stopwatch();

            logger.LogInformation("Lottery is being reseted");

            stopwatch.Start();

            try
            {
                lp.ResetPeriod();
            }
            catch (Exception ex)
            {
                logger.LogError("Unable to reset lottery");
                return RedirectToPage();
            }

            stopwatch.Stop();

            logger.LogInformation($"Lottery was reseted, elapsed time: {stopwatch.ElapsedMilliseconds}");

            return RedirectToPage();
        }

        //draw winning numbers
        public IActionResult OnPostDrawWinners()
        {
            var stopWatch = new Stopwatch();
            logger.LogInformation("Drawing Process Has started");

            stopWatch.Start();

            try
            {
                lp.ClosePeriodSales();
                lp.p.DrawWinningTicket();
                lp.p.ComputeWinners();
            }
            catch (Exception ex)
            {
                logger.LogError("Unable to draw winners");
                return RedirectToPage();
            }

            stopWatch.Stop();

            logger.LogInformation($"Winners were drawn succesfully. Elapsed time: {stopWatch.ElapsedMilliseconds}");

            return RedirectToPage();
        }
    }
}
