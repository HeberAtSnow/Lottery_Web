using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class QuickPickModel : PageModel
    {
        public string Name { get; set; }

        public async Task OnGetAsync()
        {
            Name = TempData["Name"].ToString();
            TempData.Keep();
        }
        public IActionResult OnPost()
        {
           return RedirectToPage("/Results", new { name = Name });
        }
    }
}
