﻿namespace Falcon.MtG.DBSync
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.UserSecrets;

    internal class Program
    {
        public static IConfigurationRoot configuration;

        private static async Task Main(string[] args)
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();

            if (args.Contains("/?") || args.Contains("/help"))
            {
                Console.WriteLine("USAGE: MtGSync.exe [/?] [/force]");
                Console.WriteLine("/?: Displays this help.");
                Console.WriteLine("/force: Forces download of all set files.");
            }
            else
            {
                bool force = args.Contains("/force");

                var timer = new Stopwatch();
                timer.Start();

                var synchronizer = new DBSynchronizer(configuration.GetConnectionString("MtGDBContext"));
                await synchronizer.Sync(force);

                Console.WriteLine("Database sync completed in " + timer.Elapsed);
                timer.Stop();
            }

#if DEBUG
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
#endif
        }
    }
}