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
    public class ResultDisplayModel : PageModel
    {
        LotteryProgram lp;
        public bool testBool { get; set; }
        public bool historyBool;
        public IEnumerable<LotteryTicket> lotteryTickets;
        public DateTime LotteryPeriodTS;
        private ILogger<ResultDisplayModel> EnvironmentLogger;
 
        public ResultDisplayModel(LotteryProgram lotteryProgram, ILogger<ResultDisplayModel> logger)
        {
            lp = lotteryProgram;
            EnvironmentLogger = logger;
            LotteryPeriodTS = lp.p.PeriodBeginTS;
        }

        public void OnGet(bool salesClosed)
        {
            testBool = salesClosed;
            EnvironmentLogger.LogInformation("Results Page Accessed @" + DateTime.Now);
        }

        public void OnPostProcessDrawing()
        {
            try
            {
                EnvironmentLogger.LogInformation("Process Drawing clicked");
                testBool = lp.ClosePeriodSales();
                lp.p.DrawWinningTicket();
                lp.p.ComputeWinners();
                lotteryTickets = lp.p.ResultsByWinLevel();
                EnvironmentLogger.LogInformation("Drawing Processed @" + DateTime.Now);
            }
            catch(Exception ex)
            {
                EnvironmentLogger.LogError(ex.Message);
            }
        }

        public void OnPostLotteryResults()
        {

                EnvironmentLogger.LogInformation("Display Lottery Results clicked");
                lotteryTickets = lp.p.ResultsByWinLevel();
                EnvironmentLogger.LogInformation("Displayed results for period:" + lp.p.PeriodBeginTS);


        }

        public void OnPostHistory()
        {
            EnvironmentLogger.LogInformation("Historical Stats clicked");
            historyBool = true;
        }



        public IActionResult OnPostReturnToSettings()
        {
            return RedirectToPage("./Settings", testBool);
        }
    }
}
