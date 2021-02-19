using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace FrontEnd.Pages
{
    public class StoreModel : PageModel
    {
        private IMemoryCache _cache;
        private LotteryProgram lp;


        public IEnumerable<LotteryTicket> PurchasedTickets;


       
        public bool incorrectName = false;

        public bool incorrectTicket = false;

        public string PlayerNombre;
        public int NumQuickPicks;

        public string Selection;
        

        public StoreModel(IMemoryCache cache, LotteryProgram prog)
        {
            _cache = cache;
            lp = prog;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostQuickPick(string name)
        {
            PlayerNombre = name;
            Selection = "QuickPick";
            PurchasedTickets = lp.p.ResultsByPlayer(name);

            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            Selection = "NumberPick";
            PlayerNombre = name;
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name, int numTickets)
        {
            //START HERE

            PlayerNombre = name;
            NumQuickPicks = numTickets;
            Selection = "QuickPick";
          
            lp.lv.SellQuickTickets(name, numTickets);
            PurchasedTickets = lp.p.ResultsByPlayer(name);

            return Page();
        }

        public IActionResult OnPostNumberPickPurchase(string name, int[] ticket)
        {
            PlayerNombre = name;

            Selection = "NumberPick";

            lp.lv.SellTicket(name, ticket);

            PurchasedTickets = lp.p.ResultsByPlayer(name);


            return Page();
        }
    }
}
