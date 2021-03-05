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
            logger.LogInformation("entered the lottery store");
        }

        public IActionResult OnPostQuickPick(string name)
        {
            logger.LogInformation("clicked on Quick Pick. The Users name is {name}", name);
            PlayerNombre = name;
            Selection = "QuickPick";
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            logger.LogInformation("clicked on Number Pick. The Users name is {name}", name);
            PlayerNombre = name;
            Selection = "NumberPick";
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name,int numTickets)
        {

            logger.LogInformation("clicked on Quick Pick buy tikets. The Users name is {name}, they tried to buy {numTickets}", name, numTickets);
            var startTime = DateTime.Now;
            try
            {
                PlayerNombre = name;
                Selection = "QuickPick";
                NumQuickPicks = numTickets;
                lp.lv.SellQuickTickets(name, numTickets);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            var endTime = DateTime.Now;
            var elapsed = endTime -startTime;
            logger.LogInformation("the time to purchas {numTickets} in a quick pick was {elapsed}", numTickets, elapsed);
            return Page();
        }

        public IActionResult OnPostNumberPickPurchase(string name, int [] ticket)
        {
            logger.LogInformation("clicked on number Pick buy tikets. The Users name is {name}", name);
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
                logger.LogError(e.Message);
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name); 
            if(PurchasedTickets.Count()==0 || PurchasedTickets == null)
            {
                logger.LogWarning("no purchased tickets");
            }
            else
            {
                logger.LogInformation("succeful purchase of ticket");
            }
            return Page();
        }
    }
}
