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
                logger.LogWarning("Name is empty or null");
                return Page();
            }

            logger.LogDebug($"{name} clicked Quick Picks");

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
                logger.LogWarning("Name is empty or null");
                return Page();
            }

            logger.LogDebug($"{name} clicked Number Picks");

            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name,int numTickets)
        {
            logger.LogDebug($"{name} clicked give me them tickets");

            try
            {
                PlayerNombre = name;
                Selection = "QuickPick";
                NumQuickPicks = numTickets;

                if (string.IsNullOrWhiteSpace(name))
                {
                    logger.LogWarning("Name is empty or null");
                    return Page();
                }

                lp.lv.SellQuickTickets(name, numTickets);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                logger.LogError($"Unable to sell {numTickets} tickets to player {name}");
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            logger.LogDebug($"Player {name} bought {numTickets} quick pick tickets");
            return Page();
        }

        public IActionResult OnPostNumberPickPurchase(string name, int [] ticket)
        {
            logger.LogDebug($"{name} clicked purchase number pick");
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
                    logger.LogWarning("Name is empty or null");
                    return Page();
                }

                lp.lv.SellTicket(name, ticket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                logger.LogError($"Unable to sell ticket {ticket} to player {name}");
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            logger.LogDebug($"Ticket {ticket} was sold successfully to {name}");
            return Page();
        }
    }
}
