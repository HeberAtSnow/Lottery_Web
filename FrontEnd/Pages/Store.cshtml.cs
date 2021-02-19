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
        public IEnumerable<LotteryTicket> PurchasedTickets;

        [BindProperty, Required]
        public string ParticipantName { get; set; }

        public int NumQuickPicks;
        private const string cacheSelectionKey = "Selection";
        private const string cacheLastTicketKey = "LastTicket";
        private const string cacheRecentPurchaseKey = "RecentPurchase";
        private const string cacheNameKey = "PlayerName";
        private string cacheSelectionValue;
        private int[] _lastTicket;
        private bool recentPurcahse;
        public string Selection => cacheSelectionValue ?? "";
        public int[] LastTicket => _lastTicket ?? (_lastTicket = new int[6]);
        public bool RecentPurchase => recentPurcahse;
        public string cachedName;
        public StoreModel(IMemoryCache cache,LotteryProgram prog)
        {
            _cache = cache;
            lp = prog;
        }

        public void OnGet()
        {
            if(_cache.TryGetValue(cacheNameKey, out cachedName))
            {
                ParticipantName = cachedName;
            }
            _cache.TryGetValue(cacheRecentPurchaseKey, out recentPurcahse);
            _cache.TryGetValue(cacheLastTicketKey, out _lastTicket);
            _cache.TryGetValue(cacheSelectionKey, out cacheSelectionValue);
            PurchasedTickets = lp.p.soldTickets;
        }

        public IActionResult OnPostQuickPick()
        {
            if (_cache.TryGetValue(cacheNameKey, out cachedName))
            {
                ParticipantName = cachedName;
            }
            if (ParticipantName == null || ParticipantName == "")
            {
                return Page();
            }
            else
            {
                PurchasedTickets = lp.p.ResultsByPlayer(ParticipantName);
                cachedName = ParticipantName;
                _cache.Set(cacheNameKey, cachedName, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30)));
                cacheSelectionValue = "QuickPick";
                _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30)));
                return Page();

            }
        }
        public IActionResult OnPostNumberPick()
        {
            if (_cache.TryGetValue(cacheNameKey, out cachedName))
            {
                ParticipantName = cachedName;
            }
            if (ParticipantName == null || ParticipantName == "")
            {
                return Page();
            }
            else
            {
                PurchasedTickets = lp.p.ResultsByPlayer(ParticipantName);
                cachedName = ParticipantName;
                _cache.Set(cacheNameKey, cachedName, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30)));
                cacheSelectionValue = "NumberPick";
                _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30)));
                return Page();
            }
        }

        public IActionResult OnPostQuickPickPurchase(int numTickets)
        {
            //START HERE
            //lp.lv.SellQuickTickets(____playername_____, ____qty____);

            //TODO: need to read the html variable "name"
            //      and save it to Model's private string playerNombre
            //      ensure not nul
            NumQuickPicks = numTickets;
            if (_cache.TryGetValue(cacheNameKey, out cachedName))
            {
                ParticipantName = cachedName;
            }
            cacheSelectionValue = "QuickPick";
            cachedName = ParticipantName;
            _cache.Set(cacheNameKey, cachedName, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30)));
            _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30)));
            _cache.TryGetValue(cacheSelectionKey, out cacheSelectionValue);
            //Doh! I first tried to get just this ticket sales.  Wrong!
            //What is needed is to get all ticket sales for this player-name
            //PurchasedTickets = lp.lv.SellQuickTickets(name, numTickets);//TODO: replace "x" with playerNobmre
            lp.lv.SellQuickTickets(ParticipantName, numTickets);
            PurchasedTickets = lp.p.ResultsByPlayer(ParticipantName);
            return Page();
        }
        public IActionResult OnPostBackButton()
        {
            PurchasedTickets = lp.p.soldTickets;
            cacheSelectionValue = "";
            _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30)));
            return RedirectToPage("/store");

        }

        public IActionResult OnPostNumberPickPurchase(int [] ticket)
        {
            
            if (ticket.Length == 6)
            {
                cacheSelectionValue = "NumberPick";

                _cache.Set(cacheSelectionKey, cacheSelectionValue, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30)));
                _cache.TryGetValue(cacheSelectionKey, out cacheSelectionValue);
                if (_cache.TryGetValue(cacheNameKey, out cachedName))
                {
                    ParticipantName = cachedName;
                }

                cachedName = ParticipantName;
                _cache.Set(cacheNameKey, cachedName, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30)));
                _cache.Set(cacheRecentPurchaseKey, true);
                _cache.Set(cacheLastTicketKey, ticket);
                
                lp.lv.SellTicket(ParticipantName, ticket);
                PurchasedTickets = lp.p.ResultsByPlayer(ParticipantName);
                return Page();
            }
            else
            {
                return Page();
            }

        }
    }
}
