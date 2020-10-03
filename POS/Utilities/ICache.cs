using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace POS.Utilities
{
    /// <summary>
    /// Contract for any cache implementation.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Gets the object with specified key.
        /// </summary>
        /// <param name="key">Represent the key.</param>
        /// <returns>Return the key.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords",
            MessageId = "Get", Justification = "Used to get the cached object on the basis of key")]
        object Get(string key);

        /// <summary>
        /// Puts the specified object with given key.
        /// </summary>
        /// <param name="key">Represent the key.</param>
        /// <param name="value">Represent the value.</param>
        void Put(string key, object value);

        void Set(string key, object value, MemoryCacheEntryOptions options);

        void Set(string key, object value, CancellationChangeToken changeToken);

        void Clear();

        void Remove(string key);
    }
}