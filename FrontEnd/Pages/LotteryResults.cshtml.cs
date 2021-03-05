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
    public class LotteryResultsModel : PageModel
    {
        LotteryProgram lp { get; }
        public IEnumerable<LotteryTicket> results { get; set; }
        private readonly ILogger<IndexModel> _logger;

        public LotteryResultsModel(LotteryProgram lotteryProgram, ILogger<IndexModel> logger)
        {
            lp = lotteryProgram;
            _logger = logger;
        }
        public void OnGet()
        {

            //var elapseTime = new Stopwatch();

            //_logger.LogInformation("Getting results by win level");
            //elapseTime.Start();
            //results = lp.p.ResultsByWinLevel();
            //elapseTime.Stop();

            //_logger.LogInformation($"It took {elapseTime} to get the results by win level");
            var elapsedTime = new Stopwatch();
            _logger.LogInformation("Historical stats page loaded");



            elapsedTime.Start();
            try
            {
                results = lp.p.ResultsByWinLevel();
                elapsedTime.Stop();



                _logger.LogInformation($"Historical stats were loaded in {elapsedTime.ElapsedMilliseconds}");
            }
            catch
            {
                _logger.LogError("Failed to retreive historical stats");
            }
            finally
            {
                results = lp.p.ResultsByWinLevel();
            }

        }
    }
}
