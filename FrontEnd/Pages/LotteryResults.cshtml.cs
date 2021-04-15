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
        private readonly ILogger<LotteryResultsModel> logger;

        LotteryProgram LotteryProgram { get; }
        public IEnumerable<LotteryTicket> results { get; set; }


        public LotteryResultsModel(LotteryProgram lotteryProgram, ILogger<LotteryResultsModel> logger)
        {
            LotteryProgram = lotteryProgram;
            this.logger = logger;
        }
        public IActionResult OnGet()
        {
            try
            {
                results = LotteryProgram.Period.ResultsByWinLevel();
                logger.LogDebug("[{prefix}]: Successfully retrieved results for the current lottery period",
                    LogPrefix.Stats);

                return Page();
            }
            catch (Exception ex)
            {
                logger.LogWarning("[{prefix}]: Failed to retrieve the results for the current lottery period. Reason: {ex}",
                    LogPrefix.Stats, ex);

                return RedirectToPage("./Error");
            }
        }
    }
}
