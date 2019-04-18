using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using West.Presence.CMA.Core.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;

namespace West.Presence.CMA.Core.Tests.Helper
{
    public class CacheProviderTests : IDisposable
    {
        CacheProvider _cacheProvider;
        public CacheProviderTests()
        {
            var provider = new ServiceCollection()
                .AddDistributedMemoryCache()
                //.AddDistributedRedisCache(option =>
                //{
                //    option.Configuration = "localhost";
                //})
                .BuildServiceProvider();

            var cache = provider.GetService<IDistributedCache>();

            _cacheProvider = new CacheProvider(cache);
        }

        [Fact]
        public void Failed_On_Set_NonSeriableObject()
        {
            var testObject = new
            {
                id = 1231,
                name = "asdfa",
                time = DateTime.UtcNow
            };
            var result = _cacheProvider.Add("No_Seriable_Object", testObject, 100);

            Assert.False(result);
        }

        [Fact]
        public void Okay_On_Set_SeriableObject()
        {
            var testObject = new tempObject()
            {
                id = 1231,
                name = "asdfa",
                time = DateTime.UtcNow
            };
            var result = _cacheProvider.Add("Seriable_Object", testObject, 100);

            Assert.True(result);
        }

        [Fact]
        public void Okay_Get_Object()
        {
            var testObject = new tempObject()
            {
                id = 1231,
                name = "asdfa",
                time = DateTime.UtcNow
            };
            _cacheProvider.Add("Seriable_Object_2", testObject, 10);

            var obj = _cacheProvider.Get("Seriable_Object_2");

            Assert.NotNull(obj);
        }

        [Fact]
        public void Okay_Get_tempObject()
        {
            var testObject = new tempObject()
            {
                id = 1234,
                name = "not funny",
                time = DateTime.UtcNow
            };
            var result = _cacheProvider.Add("Seriable_Object_3", testObject, 100);

            tempObject obj = _cacheProvider.Get<tempObject>("Seriable_Object_3");

            Assert.Equal(1234, obj.id);
        }

        [Fact]
        public void Okay_Get_Object_NotExist()
        {

            var obj = _cacheProvider.Get("Seriable_Object_not_exist");

            Assert.Null(obj);
        }

        [Fact]
        public void Okay_Get_Dictionary()
        {
            _cacheProvider.Add("object1", "This is Jan", 100);
            _cacheProvider.Add("object2", "This is Feb", 100);

            var obj = _cacheProvider.Get("object1", "object2");

            Assert.Equal(2, obj.Count);
        }

        [Fact]
        public void Okay_Remove_item()
        {
            _cacheProvider.Add("to_be_removed", "This is Jan", 100);

            var obj = _cacheProvider.Remove("to_be_removed");

            Assert.True(obj);

            var obj2 = _cacheProvider.Get("to_be_removed");
            Assert.Null(obj2);
        }

        [Fact]
        public void Okay_Remove_noexist_item()
        {
            var obj = _cacheProvider.Remove("noexist_item");

            Assert.True(obj);

            //var obj2 = _cacheProvider.Get("to_be_removed");
            //Assert.Null(obj2);
        }

        [Fact]
        public void Okay_TryGetValue_Get_Existing_tempObject()
        {
            var testObject = new tempObject()
            {
                id = 1234,
                name = "not funny",
                time = DateTime.UtcNow
            };
            _cacheProvider.Add("Seriable_Object_4", testObject, 100);

            tempObject returnObject;
            var result = _cacheProvider.TryGetValue("Seriable_Object_4", out returnObject);

            Assert.True(result);
            Assert.Equal(1234, returnObject.id);
        }

        [Fact]
        public void Okay_TryGetValue_Get_NonExisting_tempObject()
        {
            tempObject returnObject;
            var result = _cacheProvider.TryGetValue("Seriable_Object_5", out returnObject);

            Assert.False(result);
            Assert.Null(returnObject);
        }

        [Fact]
        public void Okay_TryGetValue_Get_Existing_Object()
        {
            var testObject = new tempObject()
            {
                id = 1234,
                name = "not funny",
                time = DateTime.UtcNow
            };
            _cacheProvider.Add("Seriable_Object_6", testObject, 100);

            object returnObject;
            var result = _cacheProvider.TryGetValue("Seriable_Object_6", out returnObject);

            Assert.True(result);
            Assert.NotNull(returnObject);
        }

        [Fact]
        public void Okay_TryGetValue_Get_NonExisting_Object()
        {
            tempObject returnObject;
            var result = _cacheProvider.TryGetValue("Seriable_Object_7", out returnObject);

            Assert.False(result);
            Assert.Null(returnObject);
        }

        public void Dispose()
        {
            _cacheProvider.Remove("object1");
            _cacheProvider.Remove("object2");
            _cacheProvider.Remove("Seriable_Object_1");
            _cacheProvider.Remove("Seriable_Object_2");
            _cacheProvider.Remove("Seriable_Object_3");
            _cacheProvider.Remove("Seriable_Object_4");
            _cacheProvider.Remove("Seriable_Object_5");
            _cacheProvider.Remove("Seriable_Object_6");
            _cacheProvider.Remove("Seriable_Object_7");
        }
    }

    [Serializable]
    public class tempObject
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime time { get; set; }
    }
}
