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
        private readonly ILogger<IndexModel> logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            this.logger = logger;
        }

        public void OnGet()
        {
            logger.LogDebug("Index page was loaded");
        }

        public IActionResult OnPostGoToStore()
        {
            logger.LogDebug("Store button was clicked");
            return RedirectToPage("./Store");
        }
        public IActionResult OnPostGoToSettings()
        {
            logger.LogDebug("Settings button was clicked");
            return RedirectToPage("./Settings");
        }
    }
}
