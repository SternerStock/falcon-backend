namespace Falcon.MtG.DBSync
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Falcon.MtG.Models.Json;
    using Falcon.MtG.Models.Sql;
    using Microsoft.Extensions.Configuration;

    public class LegalityHelper
    {
        public IConfigurationRoot configuration;

        public LegalityHelper()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
        }

        public UpsertResult<List<Legality>> UpsertLegalities(Card card, JsonLegality legality, JsonLeadership leadership)
        {
            var result = new UpsertResult<List<Legality>>()
            {
                MainObject = new List<Legality>()
            };

            var brawl = this.UpsertLegality(card, "Brawl", IsLegal(legality.Brawl), IsLegal(legality.Brawl) && card.Types.Any(c => c.CardType.Name == "creature" || c.CardType.Name == "planeswalker") && card.Supertypes.Any(c => c.Supertype.Name == "Legendary"));
            result.MainObject.Add(brawl.MainObject);
            result.Merge(brawl);

            var commander = this.UpsertLegality(card, "Commander", IsLegal(legality.Commander), legality.Commander != "Banned" && leadership.Commander || card.Name == "Gisela, the Broken Blade");
            result.MainObject.Add(commander.MainObject);
            result.Merge(commander);

            var duel = this.UpsertLegality(card, "Duel", IsLegal(legality.Duel), legality.Duel != "Banned" && leadership.Commander || card.Name == "Gisela, the Broken Blade");
            result.MainObject.Add(duel.MainObject);
            result.Merge(duel);

            var frontier = this.UpsertLegality(card, "Frontier", IsLegal(legality.Frontier), false);
            result.MainObject.Add(frontier.MainObject);
            result.Merge(frontier);

            var future = this.UpsertLegality(card, "Future", IsLegal(legality.Future), false);
            result.MainObject.Add(future.MainObject);
            result.Merge(future);

            var legacy = this.UpsertLegality(card, "Legacy", IsLegal(legality.Legacy), false);
            result.MainObject.Add(legacy.MainObject);
            result.Merge(legacy);

            var modern = this.UpsertLegality(card, "Modern", IsLegal(legality.Modern), false);
            result.MainObject.Add(modern.MainObject);
            result.Merge(modern);

            var pauper = this.UpsertLegality(card, "Pauper", IsLegal(legality.Pauper), legality.Commander != "Banned" && card.Types.Any(c => c.CardType.Name == "creature") && card.Printings.Any(p => p.Rarity.Name == "uncommon"));
            result.MainObject.Add(pauper.MainObject);
            result.Merge(pauper);

            var penny = this.UpsertLegality(card, "Penny", IsLegal(legality.Penny), false);
            result.MainObject.Add(penny.MainObject);
            result.Merge(penny);

            var standard = this.UpsertLegality(card, "Standard", IsLegal(legality.Standard), false);
            result.MainObject.Add(standard.MainObject);
            result.Merge(standard);

            var vintage = this.UpsertLegality(card, "Vintage", IsLegal(legality.Vintage), false);
            result.MainObject.Add(vintage.MainObject);
            result.Merge(vintage);

            string obLegality = legality.Vintage;
            var OathbreakerBans = configuration.GetSection("BanLists:Oathbreaker").Get<List<string>>();
            if (OathbreakerBans.Contains(card.Name))
            {
                obLegality = "Banned";
            }

            var oathbreaker = this.UpsertLegality(card, "Oathbreaker", IsLegal(obLegality), leadership.Oathbreaker);
            result.MainObject.Add(oathbreaker.MainObject);
            result.Merge(oathbreaker);

            string tlLegality = legality.Commander;
            var TinyLeadersBans = configuration.GetSection("BanLists:TinyLeaders").Get<List<string>>();
            if (card.CMC > 3 || TinyLeadersBans.Contains(card.Name))
            {
                tlLegality = "Banned";
            }

            bool tlCmdrLegality = leadership.Commander;
            var TinyLeadersCmdrBans = configuration.GetSection("BanLists:TinyLeadersCmdr").Get<List<string>>();
            if (card.CMC > 3 || TinyLeadersCmdrBans.Contains(card.Name))
            {
                tlCmdrLegality = false;
            }

            var tinyLeaders = this.UpsertLegality(card, "TinyLeaders", IsLegal(tlLegality), tlCmdrLegality);
            result.MainObject.Add(tinyLeaders.MainObject);
            result.Merge(tinyLeaders);

            return result;
        }

        private bool IsLegal(string text)
        {
            return text == "Legal" || text == "Restricted";
        }

        private UpsertResult<Legality> UpsertLegality(Card card, string format, bool legal, bool leader)
        {
            var result = new UpsertResult<Legality>();

            var legality = card.Legalities.SingleOrDefault(l => l.Format == format);

            if (legality == null)
            {
                legality = new Legality()
                {
                    Card = card,
                    Format = format
                };

                result.ObjectsToAdd.Add(legality);
            }

            legality.Legal = legal;
            legality.LegalAsCommander = leader;

            result.MainObject = legality;

            return result;
        }
    }
}