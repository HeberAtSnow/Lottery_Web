using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
        public IEnumerable<LotteryTicket> PurchasedTickets;
        [Required]
        public string PlayerNombre;
        public int NumQuickPicks;
        private int[] _lastTicket;
        private bool recentPurchase;
        public string Selection;
        public int[] LastTicket => _lastTicket ?? (_lastTicket = new int[6]);
        public bool RecentPurchase => recentPurchase;
        Stopwatch stopwatch = new Stopwatch();

        ILogger AccessLogger = new LoggerConfiguration()
            .WriteTo.File("C:\\logs\\Access.log")
            .CreateLogger();

        ILogger PerformanceLogger = new LoggerConfiguration()
            .WriteTo.File("C:\\logs\\AppPerformance.log")
            .CreateLogger();

        public StoreModel(IMemoryCache cache,LotteryProgram prog)
        {
            lp = prog;
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPostQuickPick(string name)
        {
            AccessLogger.Information("Quick Pick Button was Clicked");
            PlayerNombre = name;
            Selection = "QuickPick";
            stopwatch.Start();
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            PerformanceLogger.Information("ResultsByPlayer() for Quick Pick Runtime: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            AccessLogger.Information("Number Pick Button was Clicked");
            PlayerNombre = name;
            Selection = "NumberPick";
            stopwatch.Start();
            PurchasedTickets = lp.p.ResultsByPlayer(name);
            PerformanceLogger.Information("ResultsByPlayer() for Number Pick Runtime: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name,int numTickets)
        {
            try
            {
                AccessLogger.Information("Quick Pick Ticket Purchase Button was Clicked");
                PlayerNombre = name;
                Selection = "QuickPick";
                NumQuickPicks = numTickets;
                stopwatch.Start();
                lp.lv.SellQuickTickets(name, numTickets);
                PerformanceLogger.Information("SellQuickTickets() Runtime: " + stopwatch.ElapsedMilliseconds);
                stopwatch.Stop();
            }
            catch (Exception e)
            {
                AccessLogger.Error(e.Message + " Occuring from OnPostQuickPickPurchase().");
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
                AccessLogger.Error(e.Message + " Occurring from OnPostNumberPickPurchase().");
                Console.WriteLine(e.Message);
                return Page();
            }
            PurchasedTickets = lp.p.ResultsByPlayer(name); 
            return Page();
        }
    }
}
