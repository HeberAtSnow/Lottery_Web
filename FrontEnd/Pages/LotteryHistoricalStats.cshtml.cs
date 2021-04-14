using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class LotteryHistoricalStatsModel : PageModel
    {
        private readonly LotteryStatistics lotteryStats;
        public IEnumerable<TicketSale> Sales { get; private set; }

        public LotteryHistoricalStatsModel(LotteryStatistics lotteryStatistics)
        {
            lotteryStats = lotteryStatistics;
        }

        public void OnGet()
        {
                Sales = lotteryStats.DBStatsAllPeriods();
        }
    }
}
