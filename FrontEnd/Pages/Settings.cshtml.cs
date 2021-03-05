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
        private readonly ILogger<IndexModel> _logger;
        public SettingsModel(LotteryProgram lotteryProgram, ILogger<IndexModel> logger)
        {
            lp = lotteryProgram;
            _logger = logger;
        }
        public void OnGet()
        {
          
        }

        public void OnPostResetLottery()
        {

            lp.ResetPeriod();
        }
        //draw winning numbers
        //current lottery results
        //all lottery statistics
    }
}
