using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{

    public class SettingsModel : PageModel
    {
        LotteryProgram lp;
        //lotteryvendor.lotteryprogram
        //lotteryprogram has a lottery period
        public string message;
        private bool periodClosed;
        public SettingsModel(LotteryProgram lotteryProgram)
        {
            lp = lotteryProgram;
        }

        public void OnGet(bool salesclosed)
        {
            periodClosed = salesclosed;
        }

        public void OnPostResetLottery()
        {
            try
            {
                if (periodClosed)
                {
                    message = "Lottery Period has been reset";
                    lp.ResetPeriod();
                }
                else
                {
                    lp.ResetPeriod();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }

        //draw winning numbers
        //current lottery results
        //all lottery statistics
    }
}
