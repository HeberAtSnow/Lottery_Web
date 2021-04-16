using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
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
            _logger.LogDebug("{prefix}: The lottery home page was successfully loaded on {dow}, {time}",
                LogPrefix.WebTraffic, DateTime.Now.ToString("ddd"), DateTime.Now);
        }

        public IActionResult OnPostGoToStore()
        {
            _logger.LogDebug("{prefix}: The button was pressed to go to the store to purchase lottery tickets on {dow}, {time}",
                LogPrefix.ButtonClick, DateTime.Now.ToString("ddd"), DateTime.Now);
            return RedirectToPage("./Store");
        }
        public IActionResult OnPostGoToSettings()
        {
            _logger.LogDebug("{prefix}: The button was pressed to go to the lottery settings page on {dow}, {time}",
                LogPrefix.ButtonClick, DateTime.Now.ToString("ddd"), DateTime.Now);
            return RedirectToPage("./Settings");
        }
    }
}
