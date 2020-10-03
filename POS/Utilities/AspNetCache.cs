using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Utilities
{
    /// <summary>
    /// Implementation of ICache using Asp.Net Cache object.
    /// </summary>
    public class AspNetCache : ICache
    {
        public IMemoryCache Cache { get; set; }

        public AspNetCache(IMemoryCache memoryCache)
        {
            Cache = memoryCache;
        }

        /// <summary>
        /// Gets the object with specified key.
        /// </summary>
        /// <param name="key">Represent the key.</param>
        /// <returns>Return the key.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords",
            MessageId = "Get", Justification = "Used to get the cached object on the basis of key")]
        public object Get(string key)
        {
            string clientKey = ClientKey();
            return Cache.Get(clientKey + key);
        }

        public void Put(string key, object value)
        {
            string clientKey = ClientKey();
            Cache.Set(clientKey + key, value, DateTimeOffset.Now.AddYears(10));

        }

        public void Set(string key, object value, MemoryCacheEntryOptions options)
        {
            string clientKey = ClientKey();
            Cache.Set(clientKey + key, value, options);
        }

        public void Set(string key, object value, CancellationChangeToken changeToken)
        {
            string clientKey = ClientKey();
            Cache.Set<object>(clientKey + key, value, changeToken);
        }

        public void Clear()
        {
            // TODO:
        }

        public void Remove(string key)
        {
            string clientKey = ClientKey();
            Cache.Remove(clientKey + key);
            // TODO:
        }

        private static string ClientKey()
        {
            return "Rajni";
        }
    }
}
