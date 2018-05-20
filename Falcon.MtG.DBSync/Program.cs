namespace Falcon.MtG.DBSync
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    internal class Program
    {
        private static void InitDataDir()
        {
            var appSetting = ConfigurationManager.AppSettings["dataDir"];
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDir, appSetting);
            var fullPath = Path.GetFullPath(path);

            Directory.CreateDirectory(fullPath);
            AppDomain.CurrentDomain.SetData("DataDirectory", fullPath);
        }

        private static void Main(string[] args)
        {
            bool force = false;
            string path = string.Empty;

            if (args.Contains("/?") || args.Contains("/help"))
            {
                Console.WriteLine("USAGE: MtGSync.exe [/?] [/path:<target directory>] [/force]");
                Console.WriteLine("/?: Displays this help.");
                Console.WriteLine("/force: Forces download of all set files.");
            }
            else
            {
                if (args.Contains("/force"))
                {
                    force = true;
                }

                var timer = new Stopwatch();
                timer.Start();

                InitDataDir();
                var workingDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

                var jsonSync = new JsonSynchronizer(workingDirectory);
                var setsToUpdate = jsonSync.Sync(force);

                Console.WriteLine("JSON download completed in " + timer.Elapsed);
                timer.Restart();

                var dbSync = new DBSynchronizer(workingDirectory);
                dbSync.Sync(setsToUpdate);

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
