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
        private string _userName;

        public void OnPostSubmitName(string name)
        {
            _userName = name;
        }
        public void OnGet()
        {
        }
    }
}
