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
            _logger.LogInformation("Page index page was loaded");
        }

        public IActionResult OnPostGoToStore()
        {
            _logger.LogInformation("Page 2 was loaded");
            return RedirectToPage("./Store");
        }
        public IActionResult OnPostGoToSettings()
        {
            _logger.LogInformation("Page 2 was loaded");
            return RedirectToPage("./Settings");
        }
    }
}
