using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class PickChoiceModel : PageModel
    {
        [TempData]
        public string Name { get; set; }

        public async Task OnGetAsync(string name)
        {
            Name = name ;
        }
        public IActionResult OnPostPickNumbers()
        {
            return RedirectToPage("./PickNumbers");
        }
        public IActionResult OnPostQuickPick()
        {
            return RedirectToPage("./QuickPick");
        }
    }
}
