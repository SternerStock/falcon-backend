namespace Falcon.MtG.DBSync
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    internal class Program
    {
        public static IConfigurationRoot configuration;

        //private static void InitDataDir()
        //{
        //    var appSetting = configuration..AppSettings["dataDir"];
        //    var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        //    var path = Path.Combine(baseDir, appSetting);
        //    var fullPath = Path.GetFullPath(path);

        //    Directory.CreateDirectory(fullPath);
        //    AppDomain.CurrentDomain.SetData("DataDirectory", fullPath);
        //}

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

                //InitDataDir();
                //var workingDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

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