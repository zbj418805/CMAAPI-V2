using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using West.Presence.CMA.Api.Model;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class CategoriesController : BaseMethods
    {
        private readonly ILogger _logger = Log.ForContext<CategoriesController>();

        public CategoriesController()
        { }

        [HttpGet("cmaapi/1/categories")]
        public IActionResult GetAll([FromQuery]QueryFilter filter)
        {
            var baseDir = AppDomain.CurrentDomain.RelativeSearchPath ?? Directory.GetCurrentDirectory();
            var file = System.IO.Path.Combine(baseDir, "App_Data", "categories.json");

            if (!System.IO.File.Exists(file))
                return NoContent();


            var categories = JsonConvert.DeserializeObject<CategoriesResponse>(System.IO.File.ReadAllText(file));
            string parentStr = GetQueryString("filter.Parent") ?? "";

            int parentId = -1;
            if (int.TryParse(parentStr, out parentId))
            {
                if (parentId > 0)
                {
                    return Ok(categories.data.Where(x => x.relationships.parent.data != null && x.relationships.parent.data.id == parentId)
                                 .ToList());
                }
                else
                {
                    return NoContent();
                }
            }
            
            return Ok(categories);
            
        }
    }
}