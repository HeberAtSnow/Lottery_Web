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
        public IActionResult OnGet()
        {
            try
            {
                results = lp.p.ResultsByWinLevel();
                _logger.LogDebug("{prefix}: Lottery Results were loaded successfully at {time}", LogPrefix.WebTraffic, DateTime.Now);
                return Page();
            }
            catch(Exception ex)
            {
                _logger.LogError("{prefix}: Failed to retreive Lottery Results at {time} with the following exception:\n{ex}", LogPrefix.WebTraffic, DateTime.Now, ex);
                return RedirectToPage("./Error");
            }
        }
    }
}
