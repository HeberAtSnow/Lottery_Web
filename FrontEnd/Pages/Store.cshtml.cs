using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Pages
{
    public class StoreModel : PageModel
    {
        private readonly LotteryProgram LotteryProgram;
        private readonly ILogger<StoreModel> logger;
        private readonly IConfiguration configuration;
        private readonly int minLogLevel;

        public IEnumerable<LotteryTicket> PurchasedTickets;

        [Required]
        public string PlayerNombre;
        public int NumQuickPicks;
        public string Selection;

        public StoreModel(LotteryProgram prog, ILogger<StoreModel> logger, IConfiguration configuration)
        {
            LotteryProgram = prog;
            this.logger = logger;
            this.configuration = configuration;
            minLogLevel = (int)Enum.Parse(typeof(LogLevel), configuration[SourceContext.Store] ?? "None");
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostQuickPick(string name)
        {
            PlayerNombre = name ?? "Anonymous";
            Selection = "QuickPick";
            PurchasedTickets = LotteryProgram.Period.ResultsByPlayer(name);

            LoggerStuff.LogDebug(logger, minLogLevel,
                $"[{LogPrefix.StoreFunc}]: Retrieving {PurchasedTickets.Count()} tickets for user {name}.");

            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            PlayerNombre = name ?? "Anonymous";
            Selection = "NumberPick";
            PurchasedTickets = LotteryProgram.Period.ResultsByPlayer(name);

            LoggerStuff.LogDebug(logger, minLogLevel,
                $"[{LogPrefix.StoreFunc}]: Retrieving {PurchasedTickets.Count()} tickets for user {name}.");

            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(string name, int numTickets)
        {
            try
            {
                PlayerNombre = name ?? "Anonymous";
                Selection = "QuickPick";
                NumQuickPicks = numTickets;
                LotteryProgram.Vendor.SellQuickTickets(name, numTickets);

                LoggerStuff.LogDebug(logger, minLogLevel,
                    $"[{LogPrefix.StoreFunc}]: Successfully sold {numTickets} {Selection} tickets to user {name}.");
            }
            catch (Exception ex)
            {
                LoggerStuff.LogWarning(logger, minLogLevel,
                    $"[{LogPrefix.StoreFunc}]: Failed to sell tickets to user {name}.",
                    ex);

                return Page();
            }
            PurchasedTickets = LotteryProgram.Period.ResultsByPlayer(name);
            return Page();
        }

        public IActionResult OnPostNumberPickPurchase(string name, int [] ticket)
        {
            try
            {
                PlayerNombre = name ?? "Anonymous";
                Selection = "NumberPick";

                LotteryProgram.Vendor.SellTicket(name, ticket);

                LoggerStuff.LogDebug(logger, minLogLevel,
                    $"[{LogPrefix.StoreFunc}]: Successfully sold {1} {Selection} tickets to user {name}.");
            }
            catch (Exception ex)
            {
                LoggerStuff.LogWarning(logger, minLogLevel,
                    $"[{LogPrefix.StoreFunc}]: Failed to sell tickets to user {name}.",
                    ex);

                return Page();
            }

            PurchasedTickets = LotteryProgram.Period.ResultsByPlayer(name);
            return Page();
        }
    }
}
