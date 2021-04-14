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
        LotteryProgram LotteryProgram { get; }
        public IEnumerable<LotteryTicket> results { get; set; }


        public LotteryResultsModel(LotteryProgram lotteryProgram )
        {
            LotteryProgram = lotteryProgram;
        }
        public void OnGet()
        {
             results = LotteryProgram.Period.ResultsByWinLevel();
        }
    }
}
