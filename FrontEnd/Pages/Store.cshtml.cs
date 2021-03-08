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
        private ILogger<StoreModel> EnvironmentLogger;
        public IEnumerable<LotteryTicket> PurchasedTickets;
        [Required]
        public string PlayerNombre;
        public int NumQuickPicks;
        private int[] _lastTicket;
        private bool recentPurchase;
        public string Selection;
        public int[] LastTicket => _lastTicket ?? (_lastTicket = new int[6]);
        public bool RecentPurchase => recentPurchase;

        public StoreModel(IMemoryCache cache, LotteryProgram prog, ILogger<StoreModel> logger)
        {
            lp = prog;
            EnvironmentLogger = logger;
        }

        public void OnGet()
        {
            EnvironmentLogger.LogInformation("Store page accessed @" + DateTime.Now);
        }

        public IActionResult OnPostQuickPick(string name)
        { if (name != null)
            {
                EnvironmentLogger.LogInformation("User: " + name + "Clicked Quick Purchase button @" + DateTime.Now);
                PlayerNombre = name;
                Selection = "QuickPick";

                PurchasedTickets = lp.p.ResultsByPlayer(name);
                return Page();
            }
            else
            {
                EnvironmentLogger.LogWarning("name variable entered @" + DateTime.Now + " was null");
                return Page();
            }


        }
        public IActionResult OnPostNumberPick(string name)
        {
            if (name != null)
            {
                EnvironmentLogger.LogInformation("User: " + name + "Clicked Manual Purchase button @" + DateTime.Now);
                PlayerNombre = name;
                Selection = "NumberPick";

                PurchasedTickets = lp.p.ResultsByPlayer(name);
                return Page();
            }
            else
            { 
                EnvironmentLogger.LogWarning("name variable entered @" + DateTime.Now + " was null");
                return Page();
            }
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

                EnvironmentLogger.LogError("User:" + name + "Error:" + e.Message);
                Console.WriteLine(e.Message);
                return Page();
            }
            EnvironmentLogger.LogInformation(name + "has successfully purchased "+ numTickets + "tickets");
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
                if(e.Message != null)
                {
                    EnvironmentLogger.LogError("User:"+ name + "Error:" + e.Message + "for attempted ticket: " + ticket.ToString());
                }
                else
                {
                    EnvironmentLogger.LogError("User:" + name + "has tried to purchase a ticket while SaleState is: CLOSED");
                }
                Console.WriteLine(e.Message);
                return Page();
            }
            EnvironmentLogger.LogInformation(name + "has successfully purchased a ticket");
            PurchasedTickets = lp.p.ResultsByPlayer(name); 
            return Page();
        }
    }
}
