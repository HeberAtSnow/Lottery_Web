using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class ResultsModel : PageModel
    {
        [BindProperty]
        public string _userName { get; set; }
        public int _ticketAmount { get; set; }
        public void OnPostAutoNumber(int ticketAmount, string userName)
        {
            _userName = userName;
            _ticketAmount = ticketAmount;
        }

    }
}
