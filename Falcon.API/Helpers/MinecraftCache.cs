namespace Falcon.API.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Runtime.Caching;
    using System.Linq;
    using System.Web.Hosting;
    using fNbt;
    using Newtonsoft.Json.Linq;

    public static class MinecraftCache
    {
        public const string PlayerDataDir = @"\\GAMESHOST\PlayerData";
        private const string AvatarSvc = "https://crafatar.com/avatars/{0}?overlay=true&size=32";
        private const string BodySvc = "https://crafatar.com/renders/body/{0}?overlay=true&scale=4";
        private const int MinutesToCacheUuid = 30;
        private const string NbtCacheKey = "MCPlayerDats";
        private const string OnlinePlayersKey = "MCOnlinePlayers";
        private const int SecondsToCacheNbtFiles = 10;
        private const int SecondsToCacheOnlineStatus = 30;
        private const string UUIDToNameSvc = "https://sessionserver.mojang.com/session/minecraft/profile/";

        public static IEnumerable<string> GetOnlinePlayers()
        {
            var cache = MemoryCache.Default;
            var cachedData = cache.Get(OnlinePlayersKey);

            if (cachedData != null)
            {
                return (IEnumerable<string>)cachedData;
            }

            string[] players = null;

            var startInfo = new ProcessStartInfo();
            var enviromentPath = System.Environment.GetEnvironmentVariable("PATH");
            
            var paths = enviromentPath.Split(';');
            var pyPath = paths.Select(x => Path.Combine(x, "py.exe"))
                               .Where(x => File.Exists(x))
                               .FirstOrDefault();

            startInfo.FileName = pyPath;
            startInfo.Arguments = "\"" + HostingEnvironment.MapPath("~/App_Data/QueryMC.py") + "\"";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            using (var process = Process.Start(startInfo))
            {
                using (var reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd().Trim();
                    players = result.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
            }

            for (int i = 0; i < players.Length; i++)
            {
                players[i] = players[i].Trim();
            }

            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(SecondsToCacheOnlineStatus) };
            cache.Set(OnlinePlayersKey, players, policy);

            return players;
        }

        public static IEnumerable<Tuple<NbtFile, DateTime>> GetPlayerData()
        {
            var cache = MemoryCache.Default;
            var cachedData = cache.Get(NbtCacheKey);

            if (cachedData != null)
            {
                return (IEnumerable<Tuple<NbtFile, DateTime>>)cachedData;
            }

            var nbtFiles = new List<Tuple<NbtFile, DateTime>>();
            var directory = new DirectoryInfo(PlayerDataDir);
            var files = directory.EnumerateFiles("*.dat");
            foreach (var file in files)
            {
                nbtFiles.Add(new Tuple<NbtFile, DateTime>(new NbtFile(file.FullName), file.LastWriteTimeUtc));
            }

            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(SecondsToCacheNbtFiles) };
            cache.Set(NbtCacheKey, nbtFiles, policy);

            return nbtFiles;
        }

        public static string GetPlayerNameByUUID(Guid uuid)
        {
            var uuidStr = uuid.ToString();
            var cache = MemoryCache.Default;
            var cachedData = cache.Get(uuidStr);

            if (cachedData != null)
            {
                return cachedData.ToString();
            }

            string playerName;

            using (var client = new WebClient())
            {
                try
                {
                    var avatarUri = new Uri(string.Format(AvatarSvc, uuidStr));
                    var avatarFilename = HostingEnvironment.MapPath(string.Format("~/Content/Minecraft/avatar-{0}.png", uuidStr));
                    client.DownloadFile(avatarUri, avatarFilename);
                }
                catch (WebException)
                {
                }

                try
                {
                    var bodyUri = new Uri(string.Format(BodySvc, uuidStr));
                    var bodyFilename = HostingEnvironment.MapPath(string.Format("~/Content/Minecraft/body-{0}.png", uuidStr));
                    client.DownloadFile(bodyUri, bodyFilename);
                }
                catch (WebException)
                {
                }

                var nameUri = new Uri(UUIDToNameSvc + uuid.ToString("N"));
                var nameJson = client.DownloadString(nameUri);

                dynamic mojangData = JObject.Parse(nameJson);
                playerName = mojangData.name;
            }

            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(MinutesToCacheUuid) };
            cache.Set(uuidStr, playerName, policy);

            return playerName;
        }
    }
}