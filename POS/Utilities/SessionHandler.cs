using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Utilities
{
    public class SessionHandler
    {
        private static ICache cacheManager;
        public static ICache cache => cacheManager;

        public SessionHandler(ICache cache)
        {
            cacheManager = cache;
        }
    }
}
