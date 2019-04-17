using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;

namespace West.Presence.CMA.Api.Controllers
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ILogger _logger = Log.ForContext<HealthController>();
        private readonly IDistributedCache _distributedCache;

        public HealthController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("api/health/ping")]
        public IActionResult Ping()
        {
            var tempObject = new tempClass()
            {
                oid = 123123,
                name = "javdi",
                time = DateTime.UtcNow
            };

            var cacheEntryOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300)
            };

            _distributedCache.Set("test_object", ObjectToByteArray(tempObject), cacheEntryOptions);

            return Ok();
        }


        [HttpGet("api/health/pong")]
        public IActionResult Pong()
        {

            var obj = ByteArrayToObject(_distributedCache.Get("test_object"));

            return Ok(obj);
        }

        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }


        private Object ByteArrayToObject(byte[] arrBytes)
        {
            if (arrBytes == null) { return null; };
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();

            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
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
