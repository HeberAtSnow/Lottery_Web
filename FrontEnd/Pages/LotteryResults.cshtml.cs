using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace FrontEnd.Pages
{
    public class LotteryResultsModel : PageModel
    {
        LotteryProgram lp { get; }
        public ILogger Logger { get; }
        public IEnumerable<LotteryTicket> results { get; set; }


        public LotteryResultsModel(LotteryProgram lotteryProgram, ILogger logger )
        {
            lp = lotteryProgram;
            Logger = logger;
        }
        public void OnGet()
        {
            Logger.Information("Getting Results for the most resentlottery period");
                results = lp.p.ResultsByWinLevel();
            Logger.Information("count of results = {num}", results.Count());
            if (results.Count() < 1)
            {
                Logger.Warning("you requested results but did not get any returned");
            }
       
        }
    }
}
