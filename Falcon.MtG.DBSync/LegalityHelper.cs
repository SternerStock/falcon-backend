namespace Falcon.MtG.DBSync
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class LegalityHelper
    {
        private const string BrawlBanListFileName = "BrawlBanned.txt";
        private const string TinyLeadersBanListFileName = "TinyLeadersBanned.txt";
        private const string TinyLeadersCmdrBanListFileName = "TinyLeadersCmdrBanned.txt";

        private List<string> BrawlBans;
        private List<string> TinyLeadersBans;
        private List<string> TinyLeadersCmdrBans;

        public LegalityHelper(string workingDirectory)
        {
            BrawlBans = GetBans(Path.Combine(workingDirectory, TinyLeadersBanListFileName));
            TinyLeadersBans = GetBans(Path.Combine(workingDirectory, TinyLeadersBanListFileName));
            TinyLeadersCmdrBans = GetBans(Path.Combine(workingDirectory, TinyLeadersCmdrBanListFileName));
        }

        public Legality GetLegality(EdhFormat format, JsonCard card)
        {
            var legality = new Legality()
            {
                Format = format.ToString(),
                Legal = false,
                LegalAsCommander = false
            };

            string baseFormat = "Vintage";

            if (format == EdhFormat.Brawl)
            {
                baseFormat = "Standard";
            }
            else if (format == EdhFormat.Commander)
            {
                baseFormat = "Commander";
            }

            var existingLegality = card.Legalities.Where(l => l.Format == baseFormat).SingleOrDefault();

            if (existingLegality != null)
            {
                if (format == EdhFormat.Brawl)
                {
                    if (this.BrawlBans.Contains(card.Name))
                    {
                        legality.Legal = legality.LegalAsCommander = false;
                    }
                    else
                    {
                        legality.Legal = legality.LegalAsCommander = true;
                    }
                }
                else if (format == EdhFormat.TinyLeaders)
                {
                    if (card.CMC <= 3)
                    {
                        legality.Legal = legality.LegalAsCommander = true;
                    }

                    if (this.TinyLeadersBans.Contains(card.Name))
                    {
                        legality.Legal = false;
                    }

                    if (this.TinyLeadersCmdrBans.Contains(card.Name))
                    {
                        legality.LegalAsCommander = false;
                    }
                }
                else
                {
                    legality.Legal = legality.LegalAsCommander = existingLegality.Legality == "Legal";
                }
            }

            return legality;
        }

        private static List<string> GetBans(string path)
        {
            var content = File.ReadAllText(path);
            var bans = content.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            return bans.Select(b => b.Trim()).ToList();
        }
    }
}