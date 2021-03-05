using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace FrontEnd.Pages
{

    public class SettingsModel : PageModel
    {
        LotteryProgram lp;
        Stopwatch stopwatch = new Stopwatch();
        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period

        ILogger AccessLogger = new LoggerConfiguration()
            .WriteTo.File("C:\\logs\\Access.log")
            .CreateLogger();

        ILogger PerformanceLogger = new LoggerConfiguration()
            .WriteTo.File("C:\\logs\\AppPerformance.log")
            .CreateLogger();

        public SettingsModel(LotteryProgram lotteryProgram)
        {
            lp = lotteryProgram;
        }
        public void OnGet()
        {
        }

        public void OnPostResetLottery()
        {
            AccessLogger.Information("Reset Lottery Button was Clicked");
            try
            {
                lp.ResetPeriod();
            }

            catch(Exception e)
            {
                PerformanceLogger.Error(e.Message + " Occurring from OnPostResetLottery().");
            }
            
        }

        public void OnPostProcessDrawing()
        {
            stopwatch.Start();
            AccessLogger.Information("Process Drawing Button was Clicked");
            try
            {
                lp.ClosePeriodSales();
                PerformanceLogger.Information("ClosePeriodSales() Runtime: " + stopwatch.ElapsedMilliseconds);
                lp.p.DrawWinningTicket();
                PerformanceLogger.Information("DrawWinningTicket() Runtime: " + stopwatch.ElapsedMilliseconds);
                lp.p.ComputeWinners();
                PerformanceLogger.Information("ComputeWinners() Runtime: " + stopwatch.ElapsedMilliseconds);
            }

            catch(Exception e)
            {
                PerformanceLogger.Error(e.Message + " Occuring from OnPostProcessDrawing().");
            }
            

            PerformanceLogger.Information("Process Drawing Runtime: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
        }
        public IActionResult OnPostLotteryResults()
        {
            AccessLogger.Information("Lottery Results Button was Clicked");
            stopwatch.Start();

            var results = lp.p.ResultsByWinLevel();   
            
            PerformanceLogger.Information("Results By Win Level Runtime: " + stopwatch.ElapsedMilliseconds);

            stopwatch.Stop(); 

            if(results == null)
            {
                PerformanceLogger.Warning("OnPostLotteryResults() is returning a null value");
            }

            return RedirectToPage("./LotteryResults", results);
        }

        public IActionResult OnPostHistoricalStats()
        {
            AccessLogger.Information("Hisotrical Stats Button was Clicked");
            LotteryStatistics ls = new LotteryStatistics();
            stopwatch.Start();

            var results = ls.DBStatsAllPeriods();
            
            PerformanceLogger.Information("Database Stats for All Periods Runtime: " + stopwatch.ElapsedMilliseconds);

            stopwatch.Stop();

            if(results == null)
            {
                PerformanceLogger.Warning("OnPostHistoricalStats() is returning a null value");
            }

            return RedirectToPage("./HistoricalStats", results);
        }
        //draw winning numbers
        //current lottery results
        //all lottery statistics
    }
}
