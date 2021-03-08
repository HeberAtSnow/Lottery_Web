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

    public class SettingsModel : PageModel
    {
        LotteryProgram lp;
        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        public string message;
        public string resultMessage;
        public bool periodClosed;
        private ILogger<SettingsModel> EnvironmentLogger;
        public SettingsModel(LotteryProgram lotteryProgram, ILogger<SettingsModel> logger)
        {
            lp = lotteryProgram;
            EnvironmentLogger = logger;
        }

        public void OnGet(bool salesclosed)
        {
            EnvironmentLogger.LogInformation("Settings Page Accessed @" + DateTime.Now);
            periodClosed = salesclosed;
        }
        //public void OnGetLotteryResult(bool salesclosed)
        //{
        //    EnvironmentLogger.LogInformation("Redirected to Settings Page from Results Page @" + DateTime.Now);
        //    periodClosed = salesclosed;
        //    resultMessage = "Cannot see results while sales are open";
        //}

        public void OnPostResetLottery()
        {
            try
            {
                if (periodClosed)
                {
                    message = "Lottery Reset Period has been reset";
                    lp.ResetPeriod();
                    EnvironmentLogger.LogInformation("Lottery Period clicked");
                    EnvironmentLogger.LogInformation("Lottery Period rest for period: " + lp.p.PeriodBeginTS + "@ " + DateTime.Now);
                }
                else
                {
                    EnvironmentLogger.LogInformation("Lottery Reset Period clicked");
                    lp.ResetPeriod();
                   
                }
            }
            catch (Exception ex)
            {
                EnvironmentLogger.LogError("Attempted Reset resulting in: " + ex.Message);
                message = ex.Message;
            }
        }

        //draw winning numbers
        //current lottery results
        //all lottery statistics
    }
}
