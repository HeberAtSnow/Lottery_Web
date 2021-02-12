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

        private LotteryTicket lotteryTicket = new LotteryTicket();
        public List<LotteryTicket> lotteryTickets = new List<LotteryTicket>();

        private const string cacheSelectionKey = "Selection";
        private const string cacheLastTicketKey = "LastTicket";
        private const string cacheRecentPurchaseKey = "RecentPurchase";
        private string cacheSelectionValue;
        private int[] _lastTicket;
        private bool recentPurcahse;
        public string Selection = "";
        public int[] LastTicket => _lastTicket ?? (_lastTicket = new int[6]);
        public bool RecentPurchase = false;


        public bool incorrectName = false;
        
        public bool incorrectTicket = false;

        public string playerName { get; private set; }

        public StoreModel(IMemoryCache cache,LotteryProgram prog)
        {
            _cache = cache;
            lp = prog;
        }

        public void OnGet()
        {
            
           
            if(_cache.TryGetValue(cacheSelectionKey, out cacheSelectionValue))
            {
                Selection = _cache.Get(cacheSelectionKey).ToString();
            }

            if (_cache.TryGetValue("incorrectname", out incorrectName))
            {
                incorrectName = (bool)_cache.Get("incorrectname");
            }
            if (_cache.TryGetValue("incorrectticket", out incorrectTicket))
            {
                incorrectTicket = (bool)_cache.Get("incorrectticket");
            }


            if (_cache.TryGetValue("lotterytickets", out lotteryTickets))
            {
                lotteryTickets = (List<LotteryTicket>) _cache.Get("lotterytickets");
            }


            if (_cache.TryGetValue(cacheRecentPurchaseKey, out recentPurcahse))
            {
                RecentPurchase = (bool)_cache.Get(cacheRecentPurchaseKey);
            }

        }

        public IActionResult OnPostQuickPick()
        {
            cacheSelectionValue = "QuickPick";
            _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));
            return RedirectToPage();
        }
        public IActionResult OnPostNumberPick()
        {
            cacheSelectionValue = "NumberPick";
            _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));
            return RedirectToPage();
        }

        public IActionResult OnPostQuickPickPurchase(string name, int numTickets)
        {
            //START HERE
            lp.lv.SellQuickTickets(name, numTickets);
            return RedirectToPage();
        }

        public IActionResult OnPostNumberPickPurchase(string name, int [] ticket)
        {
            if (name == null)
            {
                _cache.Set("incorrectname", true, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));
                return RedirectToPage();
            }
            if (ticket.Length != 6)
            {
                _cache.Set("incorrectticket", true, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));
                return RedirectToPage();
            }
            if (ticket.Length == 6)
            {
                
                /*if (_cache.TryGetValue("lotterytickets", out lotteryTickets))
                {
                    lotteryTickets = (List<LotteryTicket>)_cache.Get("lotterytickets");
                }*/

                _cache.Set(cacheRecentPurchaseKey, true);
                lotteryTicket= lp.lv.SellTicket(name, ticket);
                lotteryTickets.Add(lotteryTicket);



                _cache.Set("lotterytickets", lotteryTickets, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(300)));
            }
            return RedirectToPage();
        }
    }
}
