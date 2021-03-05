using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        Serilog.ILogger AccessLogger = new LoggerConfiguration()
            .WriteTo.File("C:\\logs\\Access.log")
            .CreateLogger();

        public void OnGet()
        {

        }

        public IActionResult OnPostGoToStore()
        {
            AccessLogger.Information("Lottery Store Button was Clicked");
            return RedirectToPage("./Store");
        }
        public IActionResult OnPostGoToSettings()
        {
            AccessLogger.Information("Lottery Settings Button was Clicked");
            return RedirectToPage("./Settings");
        }
    }
}
