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
        private readonly LotteryProgram LotteryProgram;
        private readonly ILogger<StoreModel> logger;

        public IEnumerable<LotteryTicket> PurchasedTickets;

        [Required]
        public string PlayerNombre;
        public int NumQuickPicks;
        public string Selection;

        public StoreModel(LotteryProgram prog, ILogger<StoreModel> logger)
        {
            LotteryProgram = prog;
            this.logger = logger;
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
    }
}
