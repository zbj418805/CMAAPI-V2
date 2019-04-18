using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using West.Presence.CMA.Core.Helper;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ILogger _logger = Log.ForContext<HealthController>();

        public HealthController()
        {
        }

        [HttpGet("api/health/ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

    }
}
