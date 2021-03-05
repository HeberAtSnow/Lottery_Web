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
        private readonly ILogger<StoreModel> logger;
        public IEnumerable<LotteryTicket> PurchasedTickets;
        [Required]
        public string PlayerNombre;
        public int NumQuickPicks;
        private int[] _lastTicket;
        private bool recentPurchase;
        public string Selection;
        public int[] LastTicket => _lastTicket ?? (_lastTicket = new int[6]);
        public bool RecentPurchase => recentPurchase;

        public StoreModel(IMemoryCache cache,LotteryProgram prog, ILogger<StoreModel> logger)
        {
            lp = prog;
            this.logger = logger;
        }

        public void OnGet()
        {
            logger.LogInformation("User entered the lottery store page");
        }


        public IEnumerable<LotteryTicket> getPlayerResults(string name)
        {
            logger.LogInformation("user with name: {name} called on getting player results", name);
            return lp.p.ResultsByPlayer(name);
        }

        public IActionResult OnPostQuickPick(string name)
        {
            logger.LogInformation("user with name: {name} clicked on Quick Pick", name);

            PlayerNombre = name;
            Selection = "QuickPick";
            PurchasedTickets = getPlayerResults(name);

            

            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            logger.LogInformation("user with name: {name} clicked on Number Pick", name);

            PlayerNombre = name;
            Selection = "NumberPick";
            PurchasedTickets = getPlayerResults(name);

            logger.LogInformation("user with name: {name} successfuly purchased numbePick tickets", name);

            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name,int numTickets)
        {
            logger.LogInformation("user with name: {name} clicked on Quick Pick to purchase {numTickets} tickets", name, numTickets);

            try
            {

                PlayerNombre = name;
                Selection = "QuickPick";
                NumQuickPicks = numTickets;
                lp.lv.SellQuickTickets(name, numTickets);

                logger.LogInformation("user with name: {name} successfuly purchased {numTickets} quickPick tickets", name, numTickets);
            }
            catch (Exception e)
            {
                logger.LogError("user with name: {name} clicked on Quick Pick to purchase {numTickets} tickets, but request was failed", name, numTickets);
                Console.WriteLine(e.Message);
                return Page();
            }

            PurchasedTickets = getPlayerResults(name);

            return Page();
        }

        public IActionResult OnPostNumberPickPurchase(string name, int [] ticket)
        {

            logger.LogInformation("user with name: {name} clicked on Num Pick to purchase {ticket} tickets", name, string.Join(",", ticket));

            try
            {
                PlayerNombre = name;
                Selection = "NumberPick";
                if (ticket.Length != 6)
                {
                    logger.LogInformation("user with name: {name} clicked on Num Pick to purchase {ticket} tickets, but the request was more than 6 ticket numbers", name, string.Join(",", ticket));
                    throw new Exception("Ticket length is not six.");

                }
                lp.lv.SellTicket(name, ticket);
                logger.LogInformation("user with name: {name} successfuly purchased {ticket} quickPick tickets", name, string.Join(",", ticket));

            }
            catch (Exception e)
            {
                logger.LogError("user with name: {name} clicked on Quick Pick to purchase {tickets} tickets, but request was failed", name, string.Join(",", ticket));
                Console.WriteLine(e.Message);
                return Page();
            }

            getPlayerResults(name);

            return Page();
        }
    }
}
