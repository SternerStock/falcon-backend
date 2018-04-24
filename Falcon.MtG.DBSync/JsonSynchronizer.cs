namespace Falcon.MtG.DBSync
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using Newtonsoft.Json;

    public class JsonSynchronizer
    {
        private const string ChangeLogFileName = "changelog.json";
        private const string MtgJsonUrl = "http://mtgjson.com/json/";
        private const string SetCodesFileName = "SetCodes.json";
        private const string SetExt = "-x.json";
        private const string VersionFileName = "version.json";
        private string workingDirectory;

        public JsonSynchronizer()
        {
            this.CurrentVersion = string.Empty;
            this.workingDirectory = Environment.CurrentDirectory;
        }

        public JsonSynchronizer(string workingDir)
        {
            this.CurrentVersion = string.Empty;
            this.workingDirectory = workingDir;
        }

        public string CurrentVersion { get; set; }

        /// <summary>
        /// Sync JSON Set files from MtGJson.
        /// </summary>
        /// <param name="force">If true, force download of all set files from the server.</param>
        /// <returns>A list of updated set file names.</returns>
        public IEnumerable<string> Sync(bool force)
        {
            var setsToDownload = new HashSet<string>();

            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2;)");
                var versionFilePath = Path.Combine(this.workingDirectory, VersionFileName);
                var setCodesFilePath = Path.Combine(this.workingDirectory, SetCodesFileName);

                if (File.Exists(versionFilePath))
                {
                    this.CurrentVersion = File.ReadAllText(versionFilePath).Trim('"');
                    Console.WriteLine("Current Version: " + this.CurrentVersion);
                }

                string newVersion = client.DownloadString(MtgJsonUrl + VersionFileName).Trim('"');
                Console.WriteLine("Server Version:  " + newVersion);

                if (force || string.IsNullOrEmpty(this.CurrentVersion) || this.CurrentVersion != newVersion)
                {
                    File.WriteAllText(versionFilePath, '"' + newVersion + '"');

                    client.DownloadFile(MtgJsonUrl + SetCodesFileName, setCodesFilePath);
                    List<string> setCodes = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(setCodesFilePath));

                    if (force || string.IsNullOrEmpty(this.CurrentVersion))
                    {
                        foreach (var setCode in setCodes)
                        {
                            setsToDownload.Add(this.GetSetFileName(setCode));
                        }
                    }
                    else
                    {
                        string changelogJson = client.DownloadString(MtgJsonUrl + ChangeLogFileName);
                        List<Change> changeLog = JsonConvert.DeserializeObject<List<Change>>(changelogJson);
                        changeLog.Reverse();

                        bool currentFound = false;
                        foreach (var change in changeLog)
                        {
                            if (change.Version == this.CurrentVersion)
                            {
                                currentFound = true;
                            }
                            else if (currentFound)
                            {
                                if (change.NewSetFiles != null)
                                {
                                    foreach (var set in change.NewSetFiles)
                                    {
                                        setsToDownload.Add(this.GetSetFileName(set));
                                    }
                                }

                                if (change.UpdatedSetFiles != null)
                                {
                                    foreach (var set in change.UpdatedSetFiles)
                                    {
                                        setsToDownload.Add(this.GetSetFileName(set));
                                    }
                                }
                            }
                        }

                        List<string> missingFiles = this.GetMissingFiles(setCodes);
                        foreach (var file in missingFiles)
                        {
                            setsToDownload.Add(file);
                        }
                    }
                }
                else
                {
                    List<string> setCodes = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(setCodesFilePath));
                    List<string> missingFiles = this.GetMissingFiles(setCodes);
                    if (missingFiles.Count > 0)
                    {
                        foreach (var file in missingFiles)
                        {
                            setsToDownload.Add(file);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No update required; exiting...");
                    }
                }

                if (setsToDownload.Count > 0)
                {
                    Console.WriteLine("Downloading changed/missing sets...");
                    foreach (var setFile in setsToDownload)
                    {
                        Console.WriteLine("Downloading " + setFile + "...");
                        client.DownloadFile(MtgJsonUrl + setFile, Path.Combine(this.workingDirectory, setFile));
                    }
                }
            }

            return setsToDownload;
        }

        /// <summary>
        /// Check the working directory for any missing files from the list of sets.
        /// </summary>
        /// <param name="setCodes">A list of set codes loaded from SetCodes.json.</param>
        /// <returns>A list of missing file names.</returns>
        private List<string> GetMissingFiles(List<string> setCodes)
        {
            var missingFiles = new List<string>();

            var thisFolder = new DirectoryInfo(this.workingDirectory);
            var setFiles = thisFolder.GetFiles("*" + SetExt);
            foreach (var setCode in setCodes)
            {
                bool setFound = false;
                foreach (var file in setFiles)
                {
                    if (file.Name == setCode + SetExt)
                    {
                        setFound = true;
                        break;
                    }
                }

                if (!setFound)
                {
                    missingFiles.Add(this.GetSetFileName(setCode));
                }
            }

            return missingFiles;
        }

        /// <summary>
        /// Checks a string to see if it ends with -x.json, and adds it on if not.
        /// </summary>
        /// <param name="setCode">A file name or set code.</param>
        /// <returns>A set file name ending with -x.json.</returns>
        private string GetSetFileName(string setCode)
        {
            if (!setCode.EndsWith(SetExt))
            {
                if (setCode.EndsWith("-x"))
                {
                    return setCode + ".json";
                }

                return setCode + SetExt;
            }

            return setCode;
        }
    }
}