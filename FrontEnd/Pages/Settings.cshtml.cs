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
        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        private readonly ILogger<IndexModel> _logger;

        public SettingsModel(LotteryProgram lotteryProgram, ILogger<IndexModel> logger)
        {
            lp = lotteryProgram;
            _logger = logger;
        }
        public void OnGet()
        {
            _logger.LogDebug("{Performance}: Setting page successfully loaded", "Access");
          
        }
    
        public IActionResult OnPostResetLottery()
        {
            var elapsedTime = new Stopwatch();
            _logger.LogDebug("{Performance}: The reset lottery button has been clicked", "Access");
            elapsedTime.Start();
            try
            {   
                lp.ResetPeriod();
            }
            catch(Exception ex)
            {
                _logger.LogError("{Performance}: Lottery failed to reset, exception: {ex}", "Performance", ex);
                return Page();
            }
            elapsedTime.Stop();
            _logger.LogDebug("{Performance}: Lottery successfully reset, it took {elapsedTime} milliseconds", "Performance", elapsedTime.ElapsedMilliseconds);
            return Page();
        }
        //draw winning numbers
        public IActionResult OnPostDrawWinningNumbers()
        {
            var elapsedTime = new Stopwatch();
            elapsedTime.Start();
            _logger.LogDebug("{Performance}: Clicked draw winning numbers, started drawing numbers.", "Performance");
            _logger.LogDebug("{Performance}: Clicked draw winning numbers, started drawing numbers.", "Access");
            try
            {
                lp.ClosePeriodSales();
                lp.p.DrawWinningTicket();
                lp.p.ComputeWinners();
            }
            catch(Exception ex)
            {
                _logger.LogError("{Performance}: Failed to draw winning numbers. Exception {ex}", "Performance", ex);
                return Page();
            }
            elapsedTime.Stop();
            _logger.LogDebug("{Performance}: Lottery successfully reset in {elapsedTime} milliseconds", "Performance", elapsedTime.ElapsedMilliseconds);
           
            return Page();
            
        }
        //current lottery results
        public IActionResult OnPostGoToResults()
        {

            _logger.LogDebug("{Performance}: Clicked on the lottery results button.", "Access");

             return RedirectToPage("./LotteryResults");
        }
        //all lottery statistics
        public IActionResult OnPostLotteryStatistics() {
            _logger.LogDebug("{Performance}: Clicked on the lottery statistics button.", "Access");
            return Page();
        }
    }
}
