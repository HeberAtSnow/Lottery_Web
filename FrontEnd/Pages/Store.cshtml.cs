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

            logger.LogDebug("[{prefix}]: Retrieving {num} tickets for user {name}.",
                LogPrefix.StoreFunc, PurchasedTickets.Count(), name);

            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            PlayerNombre = name ?? "Anonymous";
            Selection = "NumberPick";
            PurchasedTickets = LotteryProgram.Period.ResultsByPlayer(name);

            logger.LogDebug("[{prefix}]: Retrieving {num} tickets for user {name}.",
                LogPrefix.StoreFunc, PurchasedTickets.Count(), name);

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

                logger.LogDebug("[{prefix}]: Successfully sold {ticketsSold} {type} tickets to user {name}.",
                    LogPrefix.StoreFunc, numTickets, Selection, name);
            }
            catch (Exception ex)
            {
                logger.LogWarning("[{prefix}]: Failed to sell tickets to user {name}. Reason: {ex}.",
                    LogPrefix.StoreFunc, name, ex);

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

                logger.LogDebug("[{prefix}]: Successfully sold {ticketsSold} {type} ticket to user {user}.",
                    LogPrefix.StoreFunc, 1, Selection, name);
            }
            catch (Exception ex)
            {
                logger.LogWarning("[{prefix}]: Failed to sell tickets to user {name}. Reason: {ex}.",
                    LogPrefix.StoreFunc, name, ex);

                return Page();
            }

            PurchasedTickets = LotteryProgram.Period.ResultsByPlayer(name);
            return Page();
        }

        private void logDebug(string message)
        {
            if (minLogLevel <= (int)LogLevel.Debug)
            {
                logger.LogDebug(message);
            }
        }

        private void logInformation(string message)
        {
            if (minLogLevel <= (int)LogLevel.Information)
            {
                logger.LogInformation(message);
            }
        }

        private void logWarning(string message, Exception ex = null)
        {
            if (minLogLevel <= (int)LogLevel.Warning)
            {
                logger.LogWarning(message, ex);
            }
        }

        private void logError(string message, Exception ex = null)
        {
            if (minLogLevel <= (int)LogLevel.Error)
            {
                logger.LogError(message, ex);
            }
        }
    }
}
