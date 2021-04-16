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
        ILogger<StoreModel> _logger;
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
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogDebug("{prefix}: The store page was loaded on {dow}, {time}.", LogPrefix.WebTraffic, DateTime.Now.ToString("ddd"), DateTime.Now);
        }

        public IActionResult OnPostQuickPick(string name)
        {
            PlayerNombre = name;
            Selection = "QuickPick";
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            _logger.LogDebug("{prefix}: {playername} selected the option to buy {type} tickets on {dow}, {time}",
                LogPrefix.ButtonClick, PlayerNombre, Selection, DateTime.Now.ToString("ddd"), DateTime.Now);
            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            PlayerNombre = name;
            Selection = "NumberPick";
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            _logger.LogDebug("{prefix}: {playername} selected the option to buy {type} tickets on {dow}, {time}",
                LogPrefix.ButtonClick, PlayerNombre, Selection, DateTime.Now.ToString("ddd"), DateTime.Now);
            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name, int numTickets)
        {
            Selection = "QuickPick";
            _logger.LogDebug("{prefix}: The user has pushed the purchase {type} ticket button at {dow}, {time}",
                LogPrefix.ButtonClick, Selection, DateTime.Now.ToString("ddd"), DateTime.Now);
            try
            {
                PlayerNombre = name;
                NumQuickPicks = numTickets;
                lp.lv.SellQuickTickets(name, numTickets);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("{prefix}: Failed to sell {numTickets} {type} tickets to {name} on {dow}, {time} with the following exception: \n{ex}",
                    LogPrefix.TicketSales, numTickets, Selection, PlayerNombre, DateTime.Now.ToString("ddd"), DateTime.Now, ex);
                Console.WriteLine(ex.Message);
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);

            _logger.LogDebug("{prefix}: player {name} purchased {numTickets} {type} tickets on {dow}, {date},",
                LogPrefix.TicketSales, name, numTickets, Selection, DateTime.Now.ToString("ddd"), DateTime.Now);
            return Page();
        }

        public IActionResult OnPostNumberPickPurchase(string name, int [] ticket)
        {
            Selection = "NumberPick";
            _logger.LogDebug("{prefix}: The user has pushed the purchase {type} ticket button at {dow}, {time}",
                LogPrefix.ButtonClick, Selection, DateTime.Now.ToString("ddd"), DateTime.Now);
            try
            {
                PlayerNombre = name;

                if (ticket.Length != 6)
                {
                    throw new Exception("Ticket length is not six.");
                }
                lp.lv.SellTicket(name, ticket);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("{prefix}: Failed to sell {type} ticket with numbers {ticket} to {name} on {dow}, {time} with the following exception: \n{ex}",
                    LogPrefix.TicketSales, Selection, ticket, PlayerNombre, DateTime.Now.ToString("ddd"), DateTime.Now, ex);
                Console.WriteLine(ex.Message);
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            _logger.LogDebug("{prefix}: player {name} purchased a {type} ticket on {dow}, {date},",
                LogPrefix.TicketSales, name, Selection, DateTime.Now.ToString("ddd"), DateTime.Now);
            return Page();
        }
    }
}
