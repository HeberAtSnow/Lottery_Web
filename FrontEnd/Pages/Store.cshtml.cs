using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace FrontEnd.Pages
{
    public class StoreModel : PageModel
    {
        private LotteryProgram lp;

        public LotteryPurchaser lotteryPurchaser1;

        public IEnumerable<LotteryTicket> PurchasedTickets;

        public bool incorrectName = false;

        public bool incorrectTicket = false;

        public string PlayerNombre;
        public int NumQuickPicks;

        public string Selection;


        public StoreModel(LotteryProgram prog, LotteryPurchaser lotteryPurchaser)
        {
            lotteryPurchaser1 = lotteryPurchaser;
            lp = prog;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostQuickPick(string name)
        {
            if (name != null)
            {

                lotteryPurchaser1.PlayerName = name;
                Selection = "QuickPick";
                PurchasedTickets = lp.p.ResultsByPlayer(name);

                lotteryPurchaser1.incorrectPlayerName = false;
            }
            else
            {
                lotteryPurchaser1.incorrectPlayerName = true;
            }



            return Page();
        }
        public IActionResult OnPostNumberPick(string name)
        {
            if (name != null)
            {
                Selection = "NumberPick";
                lotteryPurchaser1.PlayerName = name;
                //PlayerNombre = name;
                PurchasedTickets = lp.p.ResultsByPlayer(name);

                lotteryPurchaser1.incorrectPlayerName = false;

            }
            else
            {
                lotteryPurchaser1.incorrectPlayerName = true;
            }



            return Page();
        }

        public IActionResult OnPostQuickPickPurchase(int numTickets)
        {
            //START HERE
            Selection = "QuickPick";
            if (numTickets != 0)
            {
                var name = lotteryPurchaser1.PlayerName;
                NumQuickPicks = numTickets;


                lp.lv.SellQuickTickets(name, numTickets);
                PurchasedTickets = lp.p.ResultsByPlayer(name);

                lotteryPurchaser1.incorrectPlayerTicket = false;
            }
            else
            {
                lotteryPurchaser1.incorrectPlayerTicket = true;
            }


            return Page();
        }

        public IActionResult OnPostNumberPickPurchase(int[] ticket)
        {

            Selection = "NumberPick";

            if (ticket.Length != 0)
            {
                var name = lotteryPurchaser1.PlayerName;



                lp.lv.SellTicket(name, ticket);

                PurchasedTickets = lp.p.ResultsByPlayer(name);

                lotteryPurchaser1.incorrectPlayerTicket = false;
            }
            else
            {
                lotteryPurchaser1.incorrectPlayerTicket = true;
            }

            return Page();
        }
    }
}
