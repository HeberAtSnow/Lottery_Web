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
            _logger.LogInformation("You are in the Index page");

            try
            {


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "We caught an exception");
            }
        }

        public IActionResult OnPostGoToStore()
        {
            return RedirectToPage("./Store");
        }
        public IActionResult OnPostGoToSettings()
        {
            return RedirectToPage("./Settings");
        }
    }
}
