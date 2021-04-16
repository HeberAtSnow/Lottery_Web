using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Pages
{
    public class StoreModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private LotteryProgram lp;
        public IEnumerable<LotteryTicket> PurchasedTickets;
        [Required]
        public string PlayerNombre;
        public int NumQuickPicks;
        private int[] _lastTicket;
        private bool recentPurchase;
        public string Selection;
        public int[] LastTicket => _lastTicket ?? (_lastTicket = new int[6]);
        public bool RecentPurchase => recentPurchase;

        public StoreModel(IMemoryCache cache,LotteryProgram prog, ILogger<IndexModel> logger)
        {
            lp = prog;
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogDebug("Store page has successfully loaded");
        }

        public IActionResult OnPostQuickPick(string name)
        {
            PlayerNombre = name;
            Selection = "QuickPick";
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            PlayerNombre = name;
            Selection = "NumberPick";
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name,int numTickets)
        {
            try
            {
                _logger.LogInformation($"{name} Asked for a QuickPick");
                
                PlayerNombre = name;
                Selection = "QuickPick";
                NumQuickPicks = numTickets;
                _logger.LogDebug($"{name} Asked for a QuickPick of {numTickets}");
                lp.lv.SellQuickTickets(name, numTickets);
            }
            catch (Exception e)
            {
                _logger.LogDebug("Quick Pick Purchase threw an exception");
                Console.WriteLine(e.Message);
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            _logger.LogDebug($"{name} successfully got tickets");
            _logger.LogInformation($"{name} successfully got tickets");
            return Page();
        }

        public IActionResult OnPostNumberPickPurchase(string name, int [] ticket)
        {
            try
            {
                PlayerNombre = name;
                Selection = "NumberPick";
                if (ticket.Length != 6)
                {
                    throw new Exception("Ticket length is not six.");
                }
                lp.lv.SellTicket(name, ticket);
            }
            catch (Exception e)
            {
                _logger.LogDebug($"An error occured while posting tickets from a Quick Pick Purchase");
                Console.WriteLine(e.Message);
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name); 
            return Page();
        }
    }
}
