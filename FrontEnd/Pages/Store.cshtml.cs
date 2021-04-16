using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace FrontEnd.Pages
{
    public class StoreModel : PageModel
    {
        private LotteryProgram lp;
        private readonly ILogger logger;
        public IEnumerable<LotteryTicket> PurchasedTickets;
        [Required]
        public string PlayerNombre;
        public int NumQuickPicks;
        private int[] _lastTicket;
        private bool recentPurchase;
        public string Selection;
        public int[] LastTicket => _lastTicket ?? (_lastTicket = new int[6]);
        public bool RecentPurchase => recentPurchase;

        public StoreModel(IMemoryCache cache, LotteryProgram prog, ILogger logger)
        {
            lp = prog;
            this.logger = logger;
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPostQuickPick(string name)
        {
            PlayerNombre = name;
            Selection = "QuickPick";
            logger.Information("{name} chose {Selection} on {date}", name, Selection, DateTime.Now);
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            PlayerNombre = name;
            Selection = "NumberPick";
            logger.Information("{name} chose {Selection} on {date}", name, Selection, DateTime.Now);
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
                logger.Information("{name} chose {Selection}  to buy {num} on {date}", name, Selection,NumQuickPicks, DateTime.Now);
                lp.lv.SellQuickTickets(name, numTickets);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                logger.Warning(e.ToString());
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            logger.Information("{count} of purchased tickets", PurchasedTickets.Count());
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
                logger.Warning(e.ToString());
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name); 
            return Page();
        }
    }
}
