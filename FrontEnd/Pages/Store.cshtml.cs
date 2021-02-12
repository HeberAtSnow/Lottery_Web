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
        public LotteryProgram lp;
        public string name;
        //keys
        private const string cacheSelectionKey = "Selection";
        private const string cacheLastTicketKey = "LastTicket";
        private const string cacheRecentPurchaseKey = "RecentPurchase";
        private const string lotteryProgramKey = "LotteryProgram";
        private const string playerNameKey = "playerName";
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
            _cache.TryGetValue(lotteryProgramKey, out lp);
            _cache.TryGetValue(playerNameKey, out name);
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

        public IActionResult OnPostQuickPickPurchase(string name, int numTickets)
        {
            if (String.IsNullOrEmpty(name) || numTickets < 1 || numTickets > 1000)
            {
                return RedirectToPage();
            }
            else
            {
                lp.lv.SellQuickTickets(name, numTickets);
                _cache.Set(cacheRecentPurchaseKey,true , new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
                _cache.Set(playerNameKey, name, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
                _cache.Set(lotteryProgramKey, lp);
                return RedirectToPage();
            }
            
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
