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
        public string PlayerNombre;
        public int NumQuickPicks;
        private const string cacheSelectionKey = "Selection";
        private const string cacheLastTicketKey = "LastTicket";
        private const string cacheRecentPurchaseKey = "RecentPurchase";
        private string cacheSelectionValue;
        private int[] _lastTicket;
        private bool recentPurcahse;
        public string Selection => cacheSelectionValue ?? "";
        public int[] LastTicket => _lastTicket ?? (_lastTicket = new int[6]);
        public bool RecentPurchase => recentPurcahse;

        public StoreModel(IMemoryCache cache,LotteryProgram prog)
        {
            _cache = cache;
            lp = prog;
        }

        public void OnGet()
        {
            _cache.TryGetValue(cacheRecentPurchaseKey, out recentPurcahse);
            _cache.TryGetValue(cacheLastTicketKey, out _lastTicket);
            _cache.TryGetValue(cacheSelectionKey, out cacheSelectionValue);
        }

        public IActionResult OnPostQuickPick(string name)
        {
            PlayerNombre = name;
            cacheSelectionValue = "QuickPick";
            _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
            return Page();
        }
        public IActionResult OnPostNumberPick()
        {
            cacheSelectionValue = "NumberPick";
            _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name,int numTickets)
        {
            //START HERE
            //lp.lv.SellQuickTickets(____playername_____, ____qty____);

            //TODO: need to read the html variable "name"
            //      and save it to Model's private string playerNombre
            //      ensure not null
            PlayerNombre = name;
            NumQuickPicks = numTickets;

            //Doh! I first tried to get just this ticket sales.  Wrong!
            //What is needed is to get all ticket sales for this player-name
            //PurchasedTickets = lp.lv.SellQuickTickets(name, numTickets);//TODO: replace "x" with playerNobmre
            lp.lv.SellQuickTickets(name, numTickets);
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }

        public IActionResult OnPostNumberPickPurchase(int [] ticket)
        {
            if (ticket.Length == 6)
            {
                lp.lv.SellTicket(PlayerNombre, ticket);
                PurchasedTickets = lp.p.ResultsByPlayer(PlayerNombre);
            }
            return Page();
        }
    }
}
