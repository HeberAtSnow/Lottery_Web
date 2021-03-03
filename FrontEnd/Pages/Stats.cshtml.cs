using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClassLib;

namespace FrontEnd.Pages
{
    public class StatsModel : PageModel
    {
        public IEnumerable<TicketSale> saleStats;
        public void OnGet(IEnumerable<TicketSale> _saleStats)
        {
            saleStats = _saleStats;

        }
    }
}
