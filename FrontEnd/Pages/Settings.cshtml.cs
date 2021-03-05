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
            _logger.LogInformation("Setting page successfully loaded");
          
        }
    
        public IActionResult OnPostResetLottery()
        {
            var elapsedTime = new Stopwatch();
            _logger.LogInformation("The reset lottery button has been clicked");
            elapsedTime.Start();
            try
            {   
                lp.ResetPeriod();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Lottery failed to reset, exception: {ex}");
                return Page();
            }
            elapsedTime.Stop();
            _logger.LogInformation($"Lottery successfully reset, it took {elapsedTime.ElapsedMilliseconds} milliseconds");
            return Page();
        }
        //draw winning numbers
        public IActionResult OnPostDrawWinningNumbers()
        {
            var elapsedTime = new Stopwatch();
            elapsedTime.Start();
            _logger.LogInformation("Clicked draw winning numbers");
            try
            {
                lp.ClosePeriodSales();
                lp.p.DrawWinningTicket();
                lp.p.ComputeWinners();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to draw winning numbers. Exception {ex}");
                return Page();
            }
            elapsedTime.Stop();
            _logger.LogInformation($"Lottery successfully reset in {elapsedTime.ElapsedMilliseconds} milliseconds");
           
            return Page();
            
        }
        //current lottery results
        public IActionResult OnPostGoToResults()
        {

            _logger.LogInformation("Clicked on the lottery results button.");
                return RedirectToPage("./LotteryResults");
        }
        //all lottery statistics
        public IActionResult OnPostLotteryStatistics() {
            _logger.LogInformation("Clicked on the lottery statistics button.");
            return Page();
        }
    }
}
