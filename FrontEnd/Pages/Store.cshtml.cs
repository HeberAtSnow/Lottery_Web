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
        private readonly ILogger<IndexModel> _logger;

        public StoreModel(IMemoryCache cache,LotteryProgram prog, ILogger<IndexModel> logger)
        {
            _logger = logger;
            lp = prog;
        }

        public void OnGet()
        {
            _logger.LogDebug("{Performance}: Store page was loaded", "Access");
        }

        public IActionResult OnPostQuickPick(string name)
        {
            _logger.LogDebug("{Performance}: {name} clicked on the quickpick button", "Access", name);
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("{Performance}: Name is empty or null", "Access");
                return Page();
            }
            else
            {
                _logger.LogDebug("{Performance}: Player Name:{name}", "Access", name);
            }
            PlayerNombre = name;
            Selection = "QuickPick";
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            _logger.LogDebug("{Performance}: {name} clicked on the number pick button button", "Access", name);
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("{Performance}: Name is empty or null", "Access");
                return Page();
            }
            else
            {
                _logger.LogDebug("{Performance}: Player Name:{name}", "Access", name);
            }
            PlayerNombre = name;
            Selection = "NumberPick";
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name,int numTickets)
        {
            try
            {
                PlayerNombre = name;
                Selection = "QuickPick";
                NumQuickPicks = numTickets;
                lp.lv.SellQuickTickets(name, numTickets);
                if (string.IsNullOrWhiteSpace(name))
                {
                    _logger.LogWarning("{Performance}: Name is empty or null", "Access");
                    return Page();
                }
                else
                {
                    _logger.LogDebug("{Performance}: Player Name:{name}", "Access", name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _logger.LogError("{Performance}: Unable to sell {numTickets} ToString player {name}", "Access", numTickets, name);
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            _logger.LogInformation("{Performance}: {name} purchased {numTickets} quickpick tickets", "Access", name, numTickets);
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
                if (string.IsNullOrWhiteSpace(name))
                {
                    _logger.LogWarning("{Performance}: Name is empty or null", "Access");
                    return Page();
                }
                else
                {
                    _logger.LogDebug("{Performance}: Player Name:{name}", "Access", name);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("{Performance}: Unable to sell number pick ticket {ticket} to player {name}", "Access", ticket.ToString(), name );
                Console.WriteLine(e.Message);
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            _logger.LogDebug("{Performance}: {name} has successfully purchased a number pick ticket", "Access", name);
            return Page();
        }
    }
}
