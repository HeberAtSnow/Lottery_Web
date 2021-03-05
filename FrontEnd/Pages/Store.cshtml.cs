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

        public StoreModel(IMemoryCache cache,LotteryProgram prog, ILogger<StoreModel> logger)
        {
            lp = prog;
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Store Page was loaded");
        }

        public IActionResult OnPostQuickPick(string name)
        {
            PlayerNombre = name;
            Selection = "QuickPick";
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            _logger.LogInformation($"{name} selected quick picks");
            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            PlayerNombre = name;
            Selection = "NumberPick";
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            _logger.LogInformation($"{name} selected Number picks");
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
                _logger.LogWarning($"{name} selected quick picks and was unable to purchase");
                return Page();
            }
            _logger.LogInformation($"{name} selected quick picks and purchased {numTickets} ticket(s)");
            PurchasedTickets = lp.p.ResultsByPlayer(name);
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
                _logger.LogWarning($"{name} selected Number picks and was unable to purchase");
                return Page();
            }
            _logger.LogInformation($"{name} selected Number picks and successfully purchased a ticket");
            PurchasedTickets = lp.p.ResultsByPlayer(name); 
            return Page();
        }
    }
}
