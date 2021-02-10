using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPostGoToStore()
        {
            return RedirectToPage("./Store");
        }
        public IActionResult OnPostGoToSettings()
        {
            return RedirectToPage("./Settings");
        }
    }
}
