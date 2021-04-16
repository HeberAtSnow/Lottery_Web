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
    public class LotteryHistoricalStatsModel : PageModel
    {
        private readonly LotteryStatistics lotteryStats;
        private readonly ILogger logger;

        public IEnumerable<TicketSale> Sales { get; private set; }
        public LotteryHistoricalStatsModel(LotteryStatistics lotteryStatistics, ILogger logger)
        {
            
            lotteryStats = lotteryStatistics;
            this.logger = logger;
        }
        public void OnGet()
        {
            logger.Information("Getting all historical stats up to {date}", DateTime.Now);
            Sales = lotteryStats.DBStatsAllPeriods();
            logger.Information("you have {count} historicl stats", Sales.Count());
            if (Sales.Count() == 0)
            {
                logger.Warning("You do not have any historicle stats to report");
            }
        }
    }
}
