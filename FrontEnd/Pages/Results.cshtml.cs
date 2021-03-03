using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{

    public class ResultsModel : PageModel
    {
        LotteryPeriod lperiod;
        IEnumerable<LotteryTicket> WinnerListByPlayer;
        IEnumerable<LotteryTicket> WinnerListByWinLevel;
        IEnumerable<LotteryTicket> LoserList;
        
        public void OnGet()
        {
            lperiod.DrawWinningTicket();
            GetListOfWinners();
        }

        public void GetListOfWinners()
        {
            //foreach(var name in ) { 
            WinnerListByPlayer = lperiod.ResultsByPlayer("Justin");
            WinnerListByWinLevel = lperiod.ResultsByWinLevel();
        }
    }
}
