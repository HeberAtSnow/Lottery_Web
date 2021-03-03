using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class HistoricalStatsModel : PageModel
    {
        private readonly LotteryStatistics LotteryStatistics;
        public IEnumerable<TicketSale> Sales { get; private set; }
        
        public HistoricalStatsModel(LotteryStatistics lotteryStatistics)
        {
            LotteryStatistics = lotteryStatistics;
        }

        public void OnGet()
        {
            Sales = LotteryStatistics.DBStatsAllPeriods();
        }
    }
}
