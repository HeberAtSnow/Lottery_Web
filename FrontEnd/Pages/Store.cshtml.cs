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
        private const string cacheSelectionKey = "Selection";
        private const string cacheLastTicketKey = "LastTicket";
        private const string cacheRecentPurchaseKey = "RecentPurchase";
        private const string cacheNameSubmittedKey = "NameSubmitted";
        private const string cacheNameKey = "Name";
        private string cacheSelectionValue;
        private int[] _lastTicket;
        private bool recentPurcahse;
        private bool nameSubmitted;
        private string playerName;
        private IEnumerable<LotteryTicket> playerTickets;
        public int NumTicketsBought { get; private set; }

        public IEnumerable<LotteryTicket> PlayerTickets => playerTickets ?? (playerTickets = new List<LotteryTicket>());
        public string Selection => cacheSelectionValue ?? "";
        public int[] LastTicket => _lastTicket ?? (_lastTicket = new int[6]);
        public bool RecentPurchase => recentPurcahse;
        public bool NameSubmitted => nameSubmitted;
        public string PlayerName => playerName;

        public StoreModel(IMemoryCache cache, LotteryProgram prog)
        {
            _cache = cache;
            lp = prog;
        }

        public void OnGet()
        {
            _cache.TryGetValue(cacheNameKey, out playerName);
            _cache.TryGetValue(cacheNameSubmittedKey, out nameSubmitted);
            _cache.TryGetValue(cacheRecentPurchaseKey, out recentPurcahse);
            _cache.TryGetValue(cacheLastTicketKey, out _lastTicket);
            _cache.TryGetValue(cacheSelectionKey, out cacheSelectionValue);
            playerTickets = lp.p.SoldTicketsByName(PlayerName);
            NumTicketsBought = playerTickets.Count();
        }

        public IActionResult OnPostSubmitName(string name)
        {
            if (!(name is null))
            {
                _cache.Set(cacheNameSubmittedKey, true);
                _cache.Set(cacheNameKey, name);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostPurchaseMethod(string selection)
        {
            _cache.Set(cacheSelectionKey, selection, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
            return RedirectToPage();
        }

        public IActionResult OnPostQuickPickPurchase(string name, int numTickets)
        {
            var validPurchase = lp.lv.SellQuickTickets(name, numTickets);
            return RedirectToPage();
        }

        public IActionResult OnPostNumberPickPurchase(int [] ticket)
        {
            if (ticket.Length == 6)
            {
                _cache.Set(cacheRecentPurchaseKey, true);
                _cache.Set(cacheLastTicketKey, ticket);
            }
            return RedirectToPage();
        }
    }
}
