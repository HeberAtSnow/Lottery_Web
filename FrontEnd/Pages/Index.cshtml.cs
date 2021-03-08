using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> EnvironmentLogger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            EnvironmentLogger = logger;
        }
        public void OnGet()
        {
            EnvironmentLogger.LogInformation("Index page accessed @" + DateTime.Now);
        }

        public IActionResult OnPostGoToStore()
        {
            EnvironmentLogger.LogInformation("Store button clicked.");
            EnvironmentLogger.LogInformation("User entered Store @" + DateTime.Now);
            return RedirectToPage("./Store");
        }
        public IActionResult OnPostGoToSettings()
        {
            return RedirectToPage("./Settings");
        }
    }
}
