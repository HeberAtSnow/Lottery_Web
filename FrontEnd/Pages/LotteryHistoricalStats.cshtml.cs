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
    public class LotteryHistoricalStatsModel : PageModel
    {
        private readonly LotteryStatistics lotteryStats;
        private readonly ILogger<LotteryHistoricalStatsModel> logger;

        public IEnumerable<TicketSale> Sales { get; private set; }
        public LotteryHistoricalStatsModel(LotteryStatistics lotteryStatistics, ILogger<LotteryHistoricalStatsModel> logger)
        {
            
            lotteryStats = lotteryStatistics;
            this.logger = logger;
        }
        public void OnGet()
        {
            logger.LogTrace("Getting all historical stats up to {date}", DateTime.Now);
            Sales = lotteryStats.DBStatsAllPeriods();
            logger.LogTrace("you have {count} historicl stats", Sales.Count());
            if (Sales.Count() == 0)
            {
                logger.LogWarning("You do not have any historicle stats to report");
            }
        }
    }
}
