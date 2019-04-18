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
        private readonly ICacheProvider _cacheProvider;

        public HealthController(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        [HttpGet("api/health/ping")]
        public IActionResult Ping()
        {

        }


        [HttpGet("api/health/pong")]
        public IActionResult Pong()
        {
            var obj = _cacheProvider.Get<tempClass>("test_object");

            return Ok(obj);
        }


    }
    [Serializable]
    public class tempClass
    {
        public int oid { get; set; }
        public string name { get; set; }
        public DateTime time { get; set; }
    }
}
