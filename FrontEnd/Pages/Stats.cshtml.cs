using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClassLib;
using FrontEnd.Services;
using Serilog;

namespace FrontEnd.Pages
{
    public class StatsModel : PageModel
    {
        public IEnumerable<TicketSale> saleStats;
        private readonly IPerformanceLogger _performanceLogger;
        public StatsModel (IPerformanceLogger performanceLogger)
        {
            _performanceLogger = performanceLogger;
        }
        public void OnGet(IEnumerable<TicketSale> _saleStats, IPerformanceLogger performanceLogger)
        {
            saleStats = _saleStats;

        }
        public void OnGet ()
        {
            Log.Logger.Information("Stats page was loaded.");
        } 
    }
}
