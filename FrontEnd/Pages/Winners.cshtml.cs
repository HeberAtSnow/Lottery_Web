using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClassLib;

namespace FrontEnd.Pages
{
    public class WinnersModel : PageModel
    {
        public IEnumerable<LotteryTicket> ticketList;
        public void OnGet(List<LotteryTicket> _ticketList)
        {
            ticketList = _ticketList;
        }
    }
}
