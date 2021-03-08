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
        private readonly ILogger<StoreModel> logger;
        private LotteryProgram lp;
        public IEnumerable<LotteryTicket> PurchasedTickets;
        [Required]
        public string PlayerNombre;
        public int NumQuickPicks;
        private int[] _lastTicket;
        public string Selection;
        public int[] LastTicket => _lastTicket ?? (_lastTicket = new int[6]);

        public StoreModel(ILogger<StoreModel> logger,LotteryProgram prog)
        {
            this.logger = logger;
            lp = prog;
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPostQuickPick(string name)
        {
            Selection = "QuickPick";

            if (string.IsNullOrWhiteSpace(name))
            {
                logger.LogWarning("{File}: Name is empty or null", "[ACC]");
                return Page();
            }

            logger.LogDebug("{File}: {Name} clicked Quick Picks", "[ACC]", name);

            PlayerNombre = name;
            
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            Selection = "NumberPick";
            PlayerNombre = name;

            if (string.IsNullOrWhiteSpace(name))
            {
                logger.LogWarning("{File}: Name is empty or null", "[ACC]");
                return Page();
            }

            logger.LogDebug("{File}: {Name} clicked Number Picks", "[ACC]", name);

            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name,int numTickets)
        {
            logger.LogDebug("{File}: {Name} clicked give me them tickets", "[ACC]", name);

            try
            {
                PlayerNombre = name;
                Selection = "QuickPick";
                NumQuickPicks = numTickets;

                if (string.IsNullOrWhiteSpace(name))
                {
                    logger.LogWarning("{File}: Name is empty or null", "[ACC]");
                    return Page();
                }

                lp.lv.SellQuickTickets(name, numTickets);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                logger.LogError("{File}: Unable to sell {numTickets} tickets to player {name}", "[ACC]", numTickets, name);
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            logger.LogDebug("{File}: Player {name} bought {numTickets} quick pick tickets", "[ACC]", name, numTickets);
            return Page();
        }

        public IActionResult OnPostNumberPickPurchase(string name, int [] ticket)
        {
            logger.LogDebug("{File}: {name} clicked purchase number pick", "[ACC]", name);
            try
            {
                PlayerNombre = name;
                Selection = "NumberPick";
                if (ticket.Length != 6)
                {
                    throw new Exception("Ticket length is not six.");
                }

                if (string.IsNullOrWhiteSpace(name))
                {
                    logger.LogWarning("{File}: Name is empty or null", "[ACC]");
                    return Page();
                }

                lp.lv.SellTicket(name, ticket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                logger.LogError("{File}: Unable to sell ticket {ticket} to player {name}", "[ACC]", ticket, name);
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            logger.LogDebug("{File}: Ticket {ticket} was sold successfully to {name}", "[ACC]", ticket, name);
            return Page();
        }
    }
}
