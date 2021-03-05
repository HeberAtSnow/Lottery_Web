using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class LotteryResultsModel : PageModel
    {
        public IEnumerable<LotteryTicket> lotteryTicekts;

        public void OnGet(IEnumerable<LotteryTicket> _lotteryTickets)
        {
            lotteryTicekts = _lotteryTickets;
        }
    }
}
