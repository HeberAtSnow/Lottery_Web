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
        private const string cacheNameKey = "PlayerName";
        private const string cacheBoughtKey = "BuySuccess";
        private string cacheName;
        private string cacheSelectionValue;
        private int[] _lastTicket;
        private bool recentPurchase;
        private bool BoughtVal;


        [BindProperty]
        public string name { get; set; }
        public bool boughtVal => BoughtVal; 
        public string Selection => cacheSelectionValue ?? "";
        public int[] LastTicket => _lastTicket ?? (_lastTicket = new int[6]);
        public bool RecentPurchase => recentPurchase;

        public StoreModel(IMemoryCache cache,LotteryProgram prog)
        {
            _cache = cache;
            lp = prog;
          
        }

        public void OnGet()
        {
            _cache.TryGetValue(cacheRecentPurchaseKey, out recentPurchase);
            _cache.TryGetValue(cacheLastTicketKey, out _lastTicket);
            _cache.TryGetValue(cacheSelectionKey, out cacheSelectionValue);
            _cache.TryGetValue(cacheNameKey, out cacheName);
            _cache.TryGetValue(cacheBoughtKey, out BoughtVal);
        }

        public IActionResult OnPostQuickPick()
        {
            if (name != null)
            { 
            cacheSelectionValue = "QuickPick";
            cacheName = name;
            _cache.Set(cacheNameKey, cacheName, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(90)));
            _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
            return RedirectToPage();
            }
            else
            {
                return RedirectToPage();
            }
        }
        public IActionResult OnPostNumberPick()
        {
            if(name != null)
            {
                cacheSelectionValue = "NumberPick";
                _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));
                return RedirectToPage();
            }
            else
            {
                return RedirectToPage();
            }
        }

        public IActionResult OnPostQuickPickPurchase(int numTickets)
        {
            name = _cache.Get(cacheNameKey).ToString();
            if (name != null && numTickets >= 1 && numTickets <=1000)
            { 
                _cache.Set(cacheBoughtKey, lp.lv.SellQuickTickets(name, numTickets));
                return RedirectToPage();
            }
            else
            {
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
