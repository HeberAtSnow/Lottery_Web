using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SerilogTimings;

namespace FrontEnd.Pages
{

    public class SettingsModel : PageModel
    {
        LotteryProgram lp;
        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        private readonly ILogger<SettingsModel> _logger;
        public SettingsModel(LotteryProgram lotteryProgram, ILogger<SettingsModel> logger)
        {
            _logger = logger;
            lp = lotteryProgram;
        }
        public void OnGet()
        {
            _logger.LogTrace("Settings page was loaded");
        }

        public IActionResult OnPostResetLottery()
        {
            using (Operation.Time("Lottery Period was tried to reset "))
            {
                try
                {
                    lp.ResetPeriod();
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Lottery period failed to reset with exception {ex.Message}");
                    return Page();
                }
                _logger.LogInformation("Lottery Period was reset succesfully");
                return Page();
            }
           
        }

        public IActionResult OnPostDrawWinningNumbers()
        {
            _logger.LogTrace("The draw winning button was clicked");

            try
            {
                using(Operation.Time("Attempting to close lottery period"))
                {
                    lp.ClosePeriodSales();
                }
                using (Operation.Time("Drawing winning ticket"))
                {
                    lp.p.DrawWinningTicket();
                }
                using (Operation.Time("Computing the winners"))
                {
                    lp.p.ComputeWinners();
                }

                _logger.LogInformation($"Total tickets purchased for this period were {lp.p.ResultsByWinLevel().Count()}");
            }
            catch(Exception e)
            {
                _logger.LogError($"Failed to get a complete tasks of drawing a winning ticket with message {e.Message}");
                return Page();
            }
            _logger.LogInformation("Winning Lottery ticket was drawn succesfully");
            return Page();
        }

        public IActionResult OnPostGoToResults()
        {
            return RedirectToPage("./LotteryResults");
        }

        public IActionResult OnPostLotteryStatistics()
        {
            return RedirectToPage("./LotteryHistoricalStats");
        }
        
    }
}
