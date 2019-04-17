using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger _logger = Log.ForContext<CategoriesController>();

        public CategoriesController()
        { }

        [HttpGet("cmaapi/1/categories")]
        public IActionResult GetAll()
        {
            return Ok();
        }
    }
}