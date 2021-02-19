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
            cacheSelectionValue = "QuickPick";
            _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            cacheSelectionValue = "NumberPick";
            _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name,int numTickets)
        {
            if (String.IsNullOrEmpty(name) || numTickets <1)
            {
                return Page();
            }
            else
            {
                lp.lv.SellQuickTickets(name, numTickets);
                PurchasedTickets = lp.p.ResultsByPlayer(name);
                return Page();
            }

        }

        public IActionResult OnPostNumberPickPurchase(string name,int number1, int number2, int number3, int number4, int number5, int powerball)
        {
            if (String.IsNullOrEmpty(name))
            {
                return Page();
            }
            int[] TicketSales = new int[] { number1, number2, number3, number4, number5, powerball };
            if (TicketSales.Length == 6)
            {
                lp.lv.SellTicket(name, TicketSales);
                PurchasedTickets = lp.p.ResultsByPlayer(name);

            }
            return Page();
        }
    }
}
