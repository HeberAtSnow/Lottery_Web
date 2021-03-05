using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;
using FrontEnd.Services;
namespace FrontEnd.Pages
{

    public class SettingsModel : PageModel
    {
        LotteryProgram lp;
        LotteryStatistics lotteryStatistics = new LotteryStatistics();
        private readonly IPerformanceLogger _performanceLogger;
        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        public SettingsModel(LotteryProgram lotteryProgram, IPerformanceLogger performanceLogger)
        {
            lp = lotteryProgram;
            _performanceLogger = performanceLogger;
        }
        public void OnGet()
        {
            Log.Logger.Information("Settings page was loaded.");
        }

        public IActionResult OnPostResetLottery()
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Log.Logger.Information("Pressed reset button");
                lp.ResetPeriod();
                watch.Stop();
                _performanceLogger.Log.Information("Time to execute OnPostResetLottery(): " + watch.ElapsedMilliseconds);
                return Page();
            }
            catch (Exception e)
            {
                Log.Logger.Error("Exception caught: " + e.Message);
                _performanceLogger.Log.Error("From Settings.cshtml.cs: Exception caught: " + e.Message);
                Console.WriteLine(e.Message);
                return Page();
            }

        }
        public IActionResult OnPostDrawWinningNumbers()
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Log.Logger.Information("Pressed draw winning numbers.");
                lp.ClosePeriodSales();
                lp.p.DrawWinningTicket();
                lp.p.ComputeWinners();
                watch.Stop();
                _performanceLogger.Log.Information("Time to execute OnPostDrawWinningNumbers(): " + watch.ElapsedMilliseconds);
                return RedirectToPage("./Winners");
            }
            catch (Exception e)
            {
                Log.Logger.Error("Exception caught: " + e.Message);
                _performanceLogger.Log.Error("From Settings.cshtml.cs: Exception caught: " + e.Message);
                Console.WriteLine(e.Message);
                return Page();
            }
        }

        public IActionResult OnPostGetStatistics()
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Log.Logger.Information("Pressed get statistics button.");
                watch.Stop();
                _performanceLogger.Log.Information("Time to execute OnPostGetStatistics(): " + watch.ElapsedMilliseconds);
                return RedirectToPage("./Stats");
            }
            catch (Exception e)
            {
                Log.Logger.Error("Exception caught: " + e.Message);
                _performanceLogger.Log.Error("From Settings.cshtml.cs: Exception caught: " + e.Message);
                Console.WriteLine(e.Message);
                return Page();
            }

        }
        //draw winning numbers
        //current lottery results
        //all lottery statistics
    }
}
