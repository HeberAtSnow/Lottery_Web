using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace FrontEnd.Pages
{
    public class StoreModel : PageModel
    {
        private IMemoryCache _cache;
        private LotteryProgram lp;
        private const string cacheSelectionKey = "Selection";
        private const string cacheLastTicketKey = "LastTicket";
        private const string cacheRecentPurchaseKey = "RecentPurchase";
        private const string cachePlayerKey = "PlayerName";
        private const string cacheSoldTicketsKey = "SoldTicketsKey";
        private string cacheSelectionValue;
        private int[] _lastTicket;
        private bool recentPurcahse;
        private string _playerName;
        private IEnumerable<LotteryTicket> _playerTickets;
        public string Selection => cacheSelectionValue ?? "";
        public int[] LastTicket => _lastTicket ?? (_lastTicket = new int[6]);
        public bool RecentPurchase => recentPurcahse;
        public string PlayerName => _playerName;
        public IEnumerable<LotteryTicket> playerTickets => _playerTickets;

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
            _cache.TryGetValue(cachePlayerKey, out _playerName);
            _cache.TryGetValue(cacheSoldTicketsKey, out _playerTickets);
        }

        public IActionResult OnPostQuickPick()
        {
            cacheSelectionValue = "QuickPick";
            _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
            return RedirectToPage();
        }
        public IActionResult OnPostNumberPick()
        {
            cacheSelectionValue = "NumberPick";
            _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
            return RedirectToPage();
        }

        public IActionResult OnPostQuickPickPurchase(string playerName, int numTickets)
        {
            //START HERE
            lp.lv.SellQuickTickets(playerName, numTickets);
            var playerTickets = lp.p.GetSoldTicketsByPlayer(playerName);

            _cache.Set(cacheRecentPurchaseKey, true);
            _cache.Set(cachePlayerKey, playerName);
            _cache.Set(cacheSoldTicketsKey, playerTickets);
            
            
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
