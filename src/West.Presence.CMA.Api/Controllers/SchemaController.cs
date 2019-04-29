using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.IO;
using Newtonsoft.Json;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class SchemaController : ControllerBase
    {
        private readonly ILogger _logger = Log.ForContext<SchemaController>();
        private static string dataDirectory;

        public SchemaController() {
            var baseDir = AppDomain.CurrentDomain.RelativeSearchPath ?? Directory.GetCurrentDirectory();
            baseDir = baseDir.ToLower();
            var pathParts = baseDir.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            while (pathParts.Contains("bin"))
            {
                baseDir = Directory.GetParent(baseDir).FullName;
                pathParts = baseDir.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            }
            dataDirectory = System.IO.Path.Combine(baseDir, "App_Data", "schemas");
        }

        [HttpGet("cmaapi/1/schemas")]
        public IActionResult AllSchemas()
        {
            try
            {
                var newsSchema = GetNewsSchema() as dynamic;
                var eventsSchema = GetEventsSchema() as dynamic;
                var peopleSchema = GetPeopleSchema() as dynamic;
                var schoolsSchema = GetSchoolsSchema() as dynamic;
                if (newsSchema == null && eventsSchema == null && newsSchema == null && schoolsSchema == null )
                    return NoContent();
                else
                    return Ok(new { data = new object[] { newsSchema.data, eventsSchema.data, peopleSchema.data, schoolsSchema.data } });
                
            }
            catch (Exception ex)
            {
                _logger.Error("Get Schema Error", ex.Message);
                throw;
            }
        }

        [HttpGet("cmaapi/1/schemas/school-messenger.news")]
        public IActionResult NewsSchema()
        {
            var result = GetNewsSchema();
            if (result == null)
                return NoContent();
            else
                return Ok(result);
        }

        [HttpGet("cmaapi/1/schemas/school-messenger.events")]
        public IActionResult EventsSchema()
        {
            var result = GetEventsSchema();
            if (result == null)
                return NoContent();
            else
                return Ok(result);
        }

        [HttpGet("cmaapi/1/schemas/school-messenger.people")]
        public IActionResult PeopleSchema()
        {
            var result = GetPeopleSchema();
            if (result == null)
                return NoContent();
            else
                return Ok(result);
        }

        [HttpGet("cmaapi/1/schemas/school-messenger.schools")]
        public IActionResult SchoolsSchema()
        {
            var result = GetSchoolsSchema();
            if (result == null)
                return NoContent();
            else
                return Ok(result);
        }

        private object GetNewsSchema()
        { 
            try
            {
                var path = Path.Combine(dataDirectory, "news_schema.json");
                if (System.IO.File.Exists(path))
                    return JsonConvert.DeserializeObject(System.IO.File.ReadAllText(path));
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.Error("Get news schema error", ex.Message);
                throw;
            }
        }

        private object GetEventsSchema()
        {
            try
            {
                var path = Path.Combine(dataDirectory, "events_schema.json");
                if (System.IO.File.Exists(path))
                    return JsonConvert.DeserializeObject(System.IO.File.ReadAllText(path));
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.Error("Get events schema error", ex.Message);
                throw;
            }
        }

        private object GetSchoolsSchema()
        {
            try
            {
                var path = Path.Combine(dataDirectory, "schools_schema.json");
                if (System.IO.File.Exists(path))
                    return JsonConvert.DeserializeObject(System.IO.File.ReadAllText(path));
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.Error("Get schools schema error", ex.Message);
                throw;
            }
        }

        private object GetPeopleSchema()
        {
            try
            {
                var path = Path.Combine(dataDirectory, "people_schema.json");
                if (System.IO.File.Exists(path))
                    return JsonConvert.DeserializeObject(System.IO.File.ReadAllText(path));
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.Error("Get people schema error", ex.Message);
                throw;
            }
        }
    }
}