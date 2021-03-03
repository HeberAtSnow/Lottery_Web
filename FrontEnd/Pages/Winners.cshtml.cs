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
        LotteryProgram lp;
        public IEnumerable<LotteryTicket> ticketList;
        public WinnersModel(LotteryProgram lotteryProgram)
        {
            lp = lotteryProgram;
        }
        public void OnGet()
        {
            ticketList = lp.p.winningTicketsL.OrderByDescending(t => t.winAmtDollars);
        }
    }
}
