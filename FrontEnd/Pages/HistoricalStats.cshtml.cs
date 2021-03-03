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
        public IEnumerable<TicketSale> ticketSales;
        public void OnGet(IEnumerable<TicketSale> _ticketSales)
        {
            ticketSales = _ticketSales;
        }
    }
}
