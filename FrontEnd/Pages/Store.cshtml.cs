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
        public List<LotteryTicket> numlotteryTickets = new List<LotteryTicket>();

        public List<LotteryTicket> randlotteryTickets = new List<LotteryTicket>();

        private const string cacheSelectionKey = "Selection";
        private const string cacheLastTicketKey = "LastTicket";
        private const string cacheRecentPurchaseKey = "RecentPurchase";
        private string cacheSelectionValue;
        private int[] _lastTicket;
        //private bool numrecentPurcahse;
        public string Selection = "";
        public int[] LastTicket => _lastTicket ?? (_lastTicket = new int[6]);

        public bool NumRecentPurchase = false;
        public bool RandRecentPurchase = false;

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


            if (_cache.TryGetValue("numlotterytickets", out numlotteryTickets))
            {
                numlotteryTickets = (List<LotteryTicket>) _cache.Get("numlotterytickets");
            }

            if (_cache.TryGetValue("randlotterytickets", out randlotteryTickets))
            {
                randlotteryTickets = (List<LotteryTicket>)_cache.Get("randlotterytickets");
            }


            if (_cache.TryGetValue("numticketssold", out NumRecentPurchase))
            {
                NumRecentPurchase = (bool)_cache.Get("numticketssold");
            }

            if (_cache.TryGetValue("randticketssold", out RandRecentPurchase))
            {
                RandRecentPurchase = (bool)_cache.Get("randticketssold");
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
            if (name == null)
            {
                _cache.Set("incorrectname", true, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));
                _cache.Set("incorrectticket", false, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));
                return RedirectToPage();
            }
            if (numTickets <= 0)
            {
                _cache.Set("incorrectticket", true, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));
                _cache.Set("incorrectname", false, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));
                return RedirectToPage();
            }



            if (_cache.TryGetValue("randticketssold", out RandRecentPurchase))
            {
                randlotteryTickets = (List<LotteryTicket>)_cache.Get("randlotterytickets");
            }

            _cache.Set("incorrectname", false, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));
            _cache.Set("incorrectticket", false, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));

            _cache.Set("randticketssold", true);

            var newlotteryTickets = lp.lv.SellQuickTickets(name, numTickets);

            foreach (var ticket in newlotteryTickets)
            {
                randlotteryTickets.Add(ticket);
            }


            _cache.Set("randlotterytickets", randlotteryTickets, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(300)));
            return RedirectToPage();
        }

        public IActionResult OnPostNumberPickPurchase(string name, int [] ticket)
        {
            if (name == null)
            {
                _cache.Set("incorrectname", true, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));
                _cache.Set("incorrectticket", false, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));
                return RedirectToPage();
            }
            if (ticket.Length != 6)
            {
                _cache.Set("incorrectticket", true, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));
                _cache.Set("incorrectname", false, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));
                return RedirectToPage();
            }
          
            if (_cache.TryGetValue("numticketssold", out NumRecentPurchase))
            {
                numlotteryTickets = (List<LotteryTicket>)_cache.Get("numlotterytickets");
            }
               
            _cache.Set("incorrectname", false, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));
            _cache.Set("incorrectticket", false, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(600)));

            _cache.Set("numticketssold", true);
            lotteryTicket= lp.lv.SellTicket(name, ticket);
            numlotteryTickets.Add(lotteryTicket);



            _cache.Set("numlotterytickets", numlotteryTickets, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(300)));
            
            return RedirectToPage();
        }
    }
}
