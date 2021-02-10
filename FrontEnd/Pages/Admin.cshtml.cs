using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class AdminModel : PageModel
    {
        public void OnGet()
        {
        }
        public IActionResult OnPostResetLottery()
        {
            //TODO: Execute code here.
            return RedirectToPage("/Index");
        }
        public IActionResult OnPostProcessDrawing()
        {

            //TODO: Execute code here.
            return RedirectToPage("/Index");
        }
        public IActionResult OnPostLotteryResults()
        {

            //TODO: Execute code here.
            return RedirectToPage("/Index");
        }
        public IActionResult OnPostHistorialStats()
        {

            //TODO: Execute code here.
            return RedirectToPage("/Index");
        }

    }
}
