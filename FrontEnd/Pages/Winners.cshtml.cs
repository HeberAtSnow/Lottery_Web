using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClassLib;
using Serilog;
using FrontEnd.Services;

namespace FrontEnd.Pages
{
    public class WinnersModel : PageModel
    {
        LotteryProgram lp;
        private readonly IPerformanceLogger _performanceLogger;
        public WinnersModel(LotteryProgram lotteryProgram, IPerformanceLogger performanceLogger)
        {
            lp = lotteryProgram;
            _performanceLogger = performanceLogger;
        }
        public IEnumerable<LotteryTicket> ticketList;
        public void OnGet()
        {
            Log.Logger.Information("Winners page was loaded.");
            
            if (lp.p.SalesState == TicketSales.CLOSED)
            {
                ticketList = lp.p.winningTicketsL;
            }
            else
            {
                ticketList = null;
            }
        }
    }
}
