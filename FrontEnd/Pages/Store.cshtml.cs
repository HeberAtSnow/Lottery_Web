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
        private readonly ILogger logger;
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

        public StoreModel(ILogger logger,LotteryProgram prog)
        {
            this.logger = logger;
            lp = prog;
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPostQuickPick(string name)
        {
            logger.LogDebug("Quick Pick was selected");
            logger.LogDebug($"Player name: {name}");
            PlayerNombre = name;
            Selection = "QuickPick";
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            logger.LogDebug("Number Pick was selected");
            logger.LogDebug($"Player name: {name}");
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                logger.LogWarning($"Unable to sell {numTickets} tickets to player {name}");
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            logger.LogDebug($"Player {name} bought {numTickets} quick pick tickets");
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
                Console.WriteLine(e.Message);
                logger.LogWarning($"Unable to sell ticket {ticket.ToString()} to player {name}");
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            logger.LogDebug($"Ticket {ticket.ToString()} was sold successfully to {name}");
            return Page();
        }
    }
}
