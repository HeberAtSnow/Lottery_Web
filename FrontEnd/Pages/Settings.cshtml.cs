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
        private readonly LotteryProgram LotteryProgram;
        private readonly ILogger<SettingsModel> logger;

        public SettingsModel(LotteryProgram lotteryProgram, ILogger<SettingsModel> logger)
        {
            LotteryProgram = lotteryProgram;
            this.logger = logger;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostResetLottery()
        {
            try
            {
                LotteryProgram.ResetPeriod();
            }
            catch (Exception ex)
            {
                return RedirectToPage();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDrawWinners()
        {
            try
            {
                LotteryProgram.ClosePeriodSales();
                LotteryProgram.Period.DrawWinningTicket();
                LotteryProgram.Period.ComputeWinners();
            }
            catch (Exception ex)
            {
                return RedirectToPage();
            }

            return RedirectToPage();
        }
    }
}
