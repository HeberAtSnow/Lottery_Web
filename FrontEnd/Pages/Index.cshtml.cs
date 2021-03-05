using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;
using FrontEnd.Services;

namespace FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IPerformanceLogger _performanceLogger;
        public IndexModel(IPerformanceLogger performanceLogger)
        {
            _performanceLogger = performanceLogger;
        }
        public void OnGet()
        {
            Log.Logger.Information("Index page was loaded");
            
        }

        public IActionResult OnPostGoToStore()
        {
            Log.Logger.Information("Go to store button was pressed.");
            return RedirectToPage("./Store");
        }
        public IActionResult OnPostGoToSettings()
        {
            Log.Logger.Information("Go to settins button was pressed.");
            
            return RedirectToPage("./Settings");
        }
    }
}
