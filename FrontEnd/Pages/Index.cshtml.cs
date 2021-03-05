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
        }

        public IActionResult OnPostGoToStore()
        {
            _logger.LogInformation("Store button clicked.");
            _logger.LogInformation("User entered Store @" + DateTime.Now);
            return RedirectToPage("./Store");
        }
        public IActionResult OnPostGoToSettings()
        {
            return RedirectToPage("./Settings");
        }
    }
}
