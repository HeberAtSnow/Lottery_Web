using System;
using System.Collections.Generic;
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
        private readonly ILogger<LotteryResultsModel> _logger;

        public LotteryResultsModel(LotteryProgram lotteryProgram, ILogger<LotteryResultsModel> logger)
        {
            _logger = logger;
            lp = lotteryProgram;

        }
        public void OnGet()
        {
            _logger.LogTrace("Lottery Results page was loaded");
                results = lp.p.ResultsByWinLevel();
       
        }
    }
}
