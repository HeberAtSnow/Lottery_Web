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
    public class ErrorModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;
        public void OnGet()
        {
            _logger.LogDebug("{prefix}: You have successfully loaded the error page on {dow}, {time}", LogPrefix.WebTraffic, DateTime.Now.ToString("ddd"), DateTime.Now);
        }
    }
}
