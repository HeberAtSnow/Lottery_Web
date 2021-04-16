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

    public class SettingsModel : PageModel
    {
        LotteryProgram lp;
        private readonly ILogger logger;

        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        public SettingsModel(LotteryProgram lotteryProgram, ILogger logger)
        {
            lp = lotteryProgram;
            this.logger = logger;
        }
        public void OnGet()
        {

        }

        public void OnPostResetLottery()
        {
            logger.Information("Attempting to reset the period Data={date}", DateTime.Now);
            lp.ResetPeriod();
            logger.Information("period reset complete");
        }
        //draw winning numbers
        //current lottery results
        //all lottery statistics
    }
}
