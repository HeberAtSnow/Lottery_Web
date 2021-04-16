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
        public IEnumerable<TicketSale> Sales { get; private set; }
        private readonly ILogger<LotteryHistoricalStatsModel> _logger;
        public LotteryHistoricalStatsModel(LotteryStatistics lotteryStatistics, ILogger<LotteryHistoricalStatsModel> logger)
        {
            _logger = logger;
            lotteryStats = lotteryStatistics;
        }
        public void OnGet()
        {
            _logger.LogTrace("Lottery Historical Stats Model page loaded");
                Sales = lotteryStats.DBStatsAllPeriods();
        }
    }
}
