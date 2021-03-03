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

        public void OnGetAfterClose(bool persistor)
        {
            periodClosed = persistor;
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
                message = "Cannot reset Lottery Period while sales are ongoing, Please process drawing first";
            }
        }
        
        public IActionResult OnPostProcessDrawing()
        {
            periodClosed = lp.ClosePeriodSales();
            return RedirectToPage("./Settings", "AfterClose", new { persistor = periodClosed });

        }


        //draw winning numbers
        //current lottery results
        //all lottery statistics
    }
}
