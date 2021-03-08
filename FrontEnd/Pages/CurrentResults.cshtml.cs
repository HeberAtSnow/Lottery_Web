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
    public class CurrentResultsModel : PageModel
    {
        private readonly ILogger<CurrentResultsModel> logger;

        public LotteryProgram LotteryProgram { get; }
        public IEnumerable<LotteryTicket> Results { get; set; }

        public CurrentResultsModel(LotteryProgram lotteryProgram, ILogger<CurrentResultsModel> logger)
        {
            LotteryProgram = lotteryProgram;
            this.logger = logger;
        }

        public void OnGet()
        {
            var stopwatch = new Stopwatch();
            logger.LogDebug("Results page was loaded");
            
            stopwatch.Start();
            Results = LotteryProgram.p.ResultsByWinLevel();
            stopwatch.Stop();

            logger.LogDebug("{File}: Loaded Results by win level for current period. Elapesed time: {Time}", "[PRF]", stopwatch.ElapsedMilliseconds);
        }
    }
}
