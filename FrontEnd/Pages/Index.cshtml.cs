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
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            _logger.LogDebug("{Performance}: Index page loaded", "Access");
        }

        public IActionResult OnPostGoToStore()
        {
            _logger.LogDebug("{Performance}: Clicked on store page", "Access");
            return RedirectToPage("./Store");
        }
        public IActionResult OnPostGoToSettings()
        {
            _logger.LogDebug("{Performance}: Clicked on administration page", "Access");
            return RedirectToPage("./Settings");
        }
    }
}
