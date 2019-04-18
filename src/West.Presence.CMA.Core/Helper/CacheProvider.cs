using Microsoft.Extensions.Caching.Distributed;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace West.Presence.CMA.Core.Helper
{
    public interface ICacheProvider
    {
        bool Add(string key, object value, long durationSeconds = 0);
        object Get(string key);
        T Get<T>(string key);
        IDictionary<string, object> Get(params string[] keys);
        bool Remove(string key);
        bool TryGetValue(string key, out object value);
        bool TryGetValue<T>(string key, out object value);
    }


    public class CacheProvider : ICacheProvider
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger _logger = Log.ForContext<CacheProvider>();
        public CacheProvider(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public bool Add(string key, object value, long durationSeconds = 0)
        {
            try
            {
                if (durationSeconds == 0)
                {
                    _distributedCache.Set(key, ObjectToByteArray(value));
                }
                else
                {
                    var cacheEntryOptions = new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(durationSeconds)
                    };
                    _distributedCache.Set(key, ObjectToByteArray(value), cacheEntryOptions);
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.Error($"Error add key: {key}, value: {value}", e.Message);
                return false;
            }
        }

        public object Get(string key)
        {
            try
            {
                return ByteArrayToObject(_distributedCache.Get(key));
            }
            catch(Exception e)
            {
                _logger.Error($"Error retrieving value for key {key}", e.Message);
                return null;
            }
        }

        public T Get<T>(string key)
        {
            return (T)Get(key);
        }

        public IDictionary<string, object> Get (params string[] keys)
        {
            try {
                var parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 5 };
                var keyValues = new Dictionary<string, object>();
                Parallel.ForEach(keys, parallelOptions, (key) => {
                    keyValues[key] = Get(key);
                });
                return keyValues;
            }
            catch(Exception e)
            {
                _logger.Error($"Error retrieving value for key {keys}", e.Message);
                return null;
            }
        }

        public bool Remove(string key)
        {
            try
            {
                _distributedCache.Remove(key);
                return true;
            }catch(Exception e)
            {
                _logger.Error($"Error remove key {key}", e.Message);
                return false;
            }
        }

        

        public bool TryGetValue(string key, out object value)
        {
            value = Get(key);
            return value != null;
        }

        public bool TryGetValue<T>(string key, out object value)
        {
            value = Get<T>(key);
            return value != null;
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
}
