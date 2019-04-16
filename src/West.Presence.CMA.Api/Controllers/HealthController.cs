using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ILogger _logger = Log.ForContext<HealthController>();

        [HttpGet("api/health/Ping")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}
