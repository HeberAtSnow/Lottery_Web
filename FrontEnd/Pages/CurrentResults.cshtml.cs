using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class CurrentResultsModel : PageModel
    {

        public LotteryProgram LotteryProgram { get; }
        public IEnumerable<LotteryTicket> Results { get; set; }

        public CurrentResultsModel(LotteryProgram lotteryProgram)
        {
            LotteryProgram = lotteryProgram;
        }

        public void OnGet()
        {
            Results = LotteryProgram.p.ResultsByWinLevel();
        }
    }
}
