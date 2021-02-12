using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private string cacheSelectionValue;
        private int[] _lastTicket;
        private bool recentPurchase;
        public string Selection => cacheSelectionValue ?? "";
        public int[] LastTicket => _lastTicket ?? (_lastTicket = new int[6]);
        public bool RecentPurchase => recentPurchase;
        [Required]
        public string Name { get; set; }

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

        public IActionResult OnPostQuickPickPurchase(int numTickets)
        {
            Name = Request.Form["name"];
            if(lp.lv.SellQuickTickets(Name, numTickets))
            {
                return RedirectToPage();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostNumberPickPurchase(int [] ticket)
        {
            Name = Request.Form["name"];
            if (ticket.Length == 6)
            {
                _cache.Set(cacheRecentPurchaseKey, true);
                _cache.Set(cacheLastTicketKey, ticket);
                lp.lv.SellTicket(Name, ticket);
            }
            return RedirectToPage();
        }
    }
}
