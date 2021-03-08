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
        private readonly ILogger<SettingsModel> logger;

        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        public SettingsModel(LotteryProgram lotteryProgram, ILogger<SettingsModel> logger)
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

            logger.LogInformation("{File}: Lottery is being reseted", "[PRF]");

            stopwatch.Start();

            try
            {
                lp.ResetPeriod();
            }
            catch (Exception ex)
            {
                logger.LogError("{File}: Unable to reset lottery", "[PRF]");
                return RedirectToPage();
            }

            stopwatch.Stop();

            logger.LogInformation("{File}: Lottery was reseted. Elapsed time: {Time}:", "[PRF]", stopwatch.ElapsedMilliseconds);

            return RedirectToPage();
        }

        //draw winning numbers
        public IActionResult OnPostDrawWinners()
        {
            var stopWatch = new Stopwatch();
            logger.LogInformation("{File}: Drawing Process Has started", "[PRF]");

            stopWatch.Start();

            try
            {
                lp.ClosePeriodSales();
                lp.p.DrawWinningTicket();
                lp.p.ComputeWinners();
            }
            catch (Exception ex)
            {
                logger.LogError("{File}: Unable to draw winners", "[PRF]");
                return RedirectToPage();
            }

            stopWatch.Stop();

            logger.LogInformation("{File}: Winners were drawn succesfully. Elapsed time: {time}", "[PRF]", stopWatch.ElapsedMilliseconds);

            return RedirectToPage();
        }
    }
}
