namespace Falcon.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Web.Hosting;
    using CsvHelper;

    public static class SkaCache
    {
        private const int CacheTimeoutMinutes = 60;
        private const string MemCacheKey = "SkaData";

        public static List<SkaAnswer> GetAnswers()
        {
            var cache = MemoryCache.Default;
            var cachedData = cache.Get(MemCacheKey);

            if (cachedData == null)
            {
                Load();
                cachedData = cache.Get(MemCacheKey);
            }

            return (List<SkaAnswer>)cachedData;
        }

        private static void Load()
        {
            var path = HostingEnvironment.MapPath("~/App_Data/skaorkid.csv");
            var reader = new StreamReader(path);
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {
                var records = csv.GetRecords<SkaAnswer>().ToList();

                var policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(CacheTimeoutMinutes)
                };

                var cache = MemoryCache.Default;
                cache.Set(MemCacheKey, records, policy);
            }
        }
    }
}