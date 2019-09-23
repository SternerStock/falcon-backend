namespace Falcon.MtG.DBSync
{
    using Falcon.MtG.MtgJsonModels;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class LegalityHelper
    {
        private const string OathbreakerBanListFileName = "OathbreakerBanned.txt";
        private const string TinyLeadersBanListFileName = "TinyLeadersBanned.txt";
        private const string TinyLeadersCmdrBanListFileName = "TinyLeadersCmdrBanned.txt";

        private readonly List<string> OathbreakerBans;
        private readonly List<string> TinyLeadersBans;
        private readonly List<string> TinyLeadersCmdrBans;

        public LegalityHelper(string workingDirectory)
        {
            OathbreakerBans = GetBans(Path.Combine(workingDirectory, OathbreakerBanListFileName));
            TinyLeadersBans = GetBans(Path.Combine(workingDirectory, TinyLeadersBanListFileName));
            TinyLeadersCmdrBans = GetBans(Path.Combine(workingDirectory, TinyLeadersCmdrBanListFileName));
        }

        public List<Legality> UpsertLegalities(MTGDBContainer db, Card card, JsonLegality legality)
        {
            var legalities = new List<Legality>()
            {
                this.UpsertLegality(db, card, "Brawl", legality.Brawl, legality.Brawl),
                this.UpsertLegality(db, card, "Commander", legality.Commander, legality.Commander),
                this.UpsertLegality(db, card, "Duel", legality.Duel, legality.Duel),
                this.UpsertLegality(db, card, "Frontier", legality.Frontier, legality.Frontier),
                this.UpsertLegality(db, card, "Future", legality.Future, legality.Future),
                this.UpsertLegality(db, card, "Legacy", legality.Legacy, legality.Legacy),
                this.UpsertLegality(db, card, "Modern", legality.Modern, legality.Modern),
                this.UpsertLegality(db, card, "Pauper", legality.Pauper, legality.Pauper),
                this.UpsertLegality(db, card, "Penny", legality.Penny, legality.Penny),
                this.UpsertLegality(db, card, "Standard", legality.Standard, legality.Standard),
                this.UpsertLegality(db, card, "Vintage", legality.Vintage, legality.Vintage)
            };

            string obLegality = legality.Vintage;
            if (OathbreakerBans.Contains(card.Name))
            {
                obLegality = "Banned";
            }

            legalities.Add(this.UpsertLegality(db, card, "Oathbreaker", obLegality, obLegality));

            string tlLegality = legality.Commander;
            if (TinyLeadersBans.Contains(card.Name))
            {
                tlLegality = "Banned";
            }

            string tlCmdrLegality = legality.Commander;
            if (TinyLeadersCmdrBans.Contains(card.Name))
            {
                tlCmdrLegality = "Banned";
            }

            legalities.Add(this.UpsertLegality(db, card, "TinyLeaders", tlLegality, tlCmdrLegality));

            return legalities;
        }

        private static List<string> GetBans(string path)
        {
            var content = File.ReadAllText(path);
            var bans = content.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            return bans.Select(b => b.Trim()).ToList();
        }

        private Legality UpsertLegality(MTGDBContainer db, Card card, string format, string legalText, string legalAsCommander)
        {
            var legality = card.Legalities.Where(l => l.Format == format).SingleOrDefault();

            if (legality == null)
            {
                legality = db.Legalities.Add(new Legality()
                {
                    Card = card,
                    Format = format
                });
            }

            legality.Legal = legalText == "Legal" || legalText == "Restricted";
            legality.LegalAsCommander = legalAsCommander == "Legal" || legalAsCommander == "Restricted";

            return legality;
        }
    }
}