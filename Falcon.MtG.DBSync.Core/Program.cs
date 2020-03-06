namespace Falcon.MtG.DBSync
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;

    internal class Program
    {
        public static IConfigurationRoot configuration;

        private static async Task Main(string[] args)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            if (args.Contains("/?") || args.Contains("/help"))
            {
                Console.WriteLine("USAGE: MtGSync.exe [/?] [/path:<target directory>] [/force]");
                Console.WriteLine("/?: Displays this help.");
                Console.WriteLine("/force: Forces download of all set files.");
            }
            else
            {
                bool force = args.Contains("/force");

                var timer = new Stopwatch();
                timer.Start();

                var synchronizer = new DBSynchronizer();
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