using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LotteryController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<LotteryController> _logger;

        public LotteryController(ILogger<LotteryController> logger)
        {
            _logger = logger;
        }

    
    }
}
