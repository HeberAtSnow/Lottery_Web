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
        private LotteryProgram lp;
        public IEnumerable<LotteryTicket> PurchasedTickets;
        public string PlayerNombre;
        public int NumQuickPicks;
        private string cacheSelectionValue;
        public string Selection => cacheSelectionValue ?? "";

        public StoreModel(LotteryProgram prog)
        {
            lp = prog;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostQuickPick(string name)
        {
            PlayerNombre = name;
            cacheSelectionValue = "QuickPick";
            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            PlayerNombre = name;
            cacheSelectionValue = "NumberPick";
            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name,int numTickets)
        {
            cacheSelectionValue = "QuickPick";
            PlayerNombre = name;
            NumQuickPicks = numTickets;
            lp.lv.SellQuickTickets(name, numTickets);
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }

        public IActionResult OnPostNumberPickPurchase(string name, int [] ticket)
        {
            cacheSelectionValue = "NumberPick";
            PlayerNombre = name;
            if (ticket.Length == 6)
            {
                try
                {
                    lp.lv.SellTicket(name, ticket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }
    }
}
