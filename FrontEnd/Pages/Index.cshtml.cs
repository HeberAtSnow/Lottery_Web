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
        [BindProperty] public string userName { get; set; }
        [BindProperty] public bool autoTicket { get; set; }
        public void OnPostName()
        {
            TempData["userName"] = userName;
        }
        public void OnPostAutoNumber()
        {
            if(autoTicket == true)
            {
                autoTicket = false;
            }
            else
            {
                autoTicket = true;
            }
        }
    }
}
