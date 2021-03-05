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
using FrontEnd.Services;

namespace FrontEnd.Pages
{
    public class StoreModel : PageModel
    {
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

        private readonly IPerformanceLogger _performanceLogger;
        public StoreModel(IMemoryCache cache, LotteryProgram prog, IPerformanceLogger performanceLogger)
        {
            lp = prog;
            _performanceLogger = performanceLogger;
        }

        public void OnGet()
        {
            Log.Logger.Information("Store page was loaded.");
            
        }

        public IActionResult OnPostQuickPick(string name)
        {
            Log.Logger.Information("In the selection menu, Quick Pick was pressed.");
            PlayerNombre = name;
            Selection = "QuickPick";
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            Log.Logger.Information("In the selection menu, Number Pick was pressed.");
            PlayerNombre = name;
            Selection = "NumberPick";
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name,int numTickets)
        {
            try
            {
                Log.Logger.Information("Quick pick purchase button pressed.");
                PlayerNombre = name;
                Selection = "QuickPick";
                NumQuickPicks = numTickets;
                lp.lv.SellQuickTickets(name, numTickets);
            }
            catch (Exception e)
            {
                Log.Logger.Error("Exception caught: " + e.Message);
                _performanceLogger.Log.Error("From Store.cshtml.cs: Exception caught: " + e.Message);
                Console.WriteLine(e.Message);
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            return Page();
        }

        public IActionResult OnPostNumberPickPurchase(string name, int [] ticket)
        {
            try
            {
                Log.Logger.Information("Number pick purchase button pressed");
                PlayerNombre = name;
                Selection = "NumberPick";
                if (ticket.Length != 6)
                {
                    Log.Logger.Warning("Invalid input on PostNumberPickPurchase()");
                    throw new Exception("Ticket length is not six.");
                }
                lp.lv.SellTicket(name, ticket);
            }
            catch (Exception e)
            {
                Log.Logger.Error("Exception caught: " + e.Message);
                _performanceLogger.Log.Error("From Store.cshtml.cs: Exception caught: " + e.Message);
                Console.WriteLine(e.Message);
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name); 
            return Page();
        }
    }
}
