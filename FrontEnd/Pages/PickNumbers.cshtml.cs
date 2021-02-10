using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class PickNumbersModel : PageModel
    {
        public string Name { get; set; }

        public async Task OnGetAsync()
        {
            Name = TempData["Name"].ToString();
            TempData.Keep();
        }
        public IActionResult OnPost(int number1, int number2, int number3, int number4, int number5, int powerBall)
        {
            return RedirectToPage("/Results", new { name = Name });
        }
    }
}
