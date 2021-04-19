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
        private readonly ILogger<StoreModel> _logger;
        private readonly ILoggingService _loggingService;

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


        public StoreModel(IMemoryCache cache,LotteryProgram prog, ILogger<StoreModel> logger, ILoggingService loggingSerice)
        {
            lp = prog;
            _logger = logger;
            _loggingService = loggingSerice;
            _loggingService.SetLoggingLevel("Warning");
        }


        public IEnumerable<LotteryTicket> getPlayerResults(string name)
        {
            _logger.LogInformation("user with name: {name} called on getting player results", name);
            return lp.p.ResultsByPlayer(name);
        }

        public void OnGet()
        {
            _logger.LogInformation("You are in the Store page");
        }

        public IActionResult OnPostQuickPick(string name)
        {
            _logger.LogInformation("user with name: {name} clicked on Quick Pick", name);

            PlayerNombre = name;
            Selection = "QuickPick";
            PurchasedTickets = getPlayerResults(name);
            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            _logger.LogInformation("user with name: {name} clicked on Number Pick", name);
            PlayerNombre = name;
            Selection = "NumberPick";
            PurchasedTickets = getPlayerResults(name);

            _logger.LogInformation("user with name: {name} successfuly purchased numbePick tickets", name);

            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name,int numTickets)
        {
            _logger.LogInformation("user with name: {name} clicked on Quick Pick to purchase {numTickets} tickets", name, numTickets);

            try
            {
                PlayerNombre = name;
                Selection = "QuickPick";
                NumQuickPicks = numTickets;
                lp.lv.SellQuickTickets(name, numTickets);

                _logger.LogInformation("user with name: {name} successfuly purchased {numTickets} quickPick tickets", name, numTickets);
            }
            catch (Exception e)
            {
                _logger.LogError("user with name: {name} clicked on Quick Pick to purchase {numTickets} tickets, but request was failed. The message is: {message}", name, numTickets, e.Message);
                return Page();
            }

            PurchasedTickets = getPlayerResults(name);
            return Page();
        }

        public IActionResult OnPostNumberPickPurchase(string name, int [] ticket)
        {
            _logger.LogInformation("user with name: {name} clicked on Num Pick to purchase {ticket} tickets", name, string.Join(",", ticket));


            try
            {
                PlayerNombre = name;
                Selection = "NumberPick";
                if (ticket.Length != 6)
                {
                    _logger.LogInformation("user with name: {name} clicked on Num Pick to purchase {ticket} tickets, but the request was more than 6 ticket numbers", name, string.Join(",", ticket));
                    throw new Exception("Ticket length is not six.");
                }
                lp.lv.SellTicket(name, ticket);
                _logger.LogInformation("user with name: {name} successfuly purchased {ticket} quickPick tickets", name, string.Join(",", ticket));

            }
            catch (Exception e)
            {
                _logger.LogError("user with name: {name} clicked on Quick Pick to purchase {tickets} tickets, but request was failed with message {message}", name, string.Join(",", ticket), e.Message);
            
                return Page();
            }
            PurchasedTickets = getPlayerResults(name);
            return Page();
        }
    }
}
