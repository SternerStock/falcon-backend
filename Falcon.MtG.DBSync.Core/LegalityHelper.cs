﻿namespace Falcon.MtG.DBSync
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
                MainObject = []
            };

            var brawl = UpsertLegality(card, "Brawl", IsLegal(legality.Brawl), IsLegal(legality.Brawl) && leadership.Brawl);
            result.MainObject.Add(brawl.MainObject);
            result.Merge(brawl);

            var commander = UpsertLegality(card, "Commander", IsLegal(legality.Commander), IsLegal(legality.Commander) && leadership.Commander);
            result.MainObject.Add(commander.MainObject);
            result.Merge(commander);

            var duel = UpsertLegality(card, "Duel", IsLegal(legality.Duel), IsLegal(legality.Duel) && leadership.Commander);
            result.MainObject.Add(duel.MainObject);
            result.Merge(duel);

            var frontier = UpsertLegality(card, "Frontier", IsLegal(legality.Frontier), false);
            result.MainObject.Add(frontier.MainObject);
            result.Merge(frontier);

            var historic = UpsertLegality(card, "Historic", IsLegal(legality.Historic), false);
            result.MainObject.Add(historic.MainObject);
            result.Merge(historic);

            var historicBrawl = UpsertLegality(card, "HistoricBrawl", IsLegal(legality.HistoricBrawl), IsLegal(legality.HistoricBrawl) && leadership.Brawl);
            result.MainObject.Add(historicBrawl.MainObject);
            result.Merge(historicBrawl);

            var future = UpsertLegality(card, "Future", IsLegal(legality.Future), false);
            result.MainObject.Add(future.MainObject);
            result.Merge(future);

            var legacy = UpsertLegality(card, "Legacy", IsLegal(legality.Legacy), false);
            result.MainObject.Add(legacy.MainObject);
            result.Merge(legacy);

            var modern = UpsertLegality(card, "Modern", IsLegal(legality.Modern), false);
            result.MainObject.Add(modern.MainObject);
            result.Merge(modern);

            var pioneer = UpsertLegality(card, "Pioneer", IsLegal(legality.Pioneer), false);
            result.MainObject.Add(pioneer.MainObject);
            result.Merge(pioneer);

            var pauper = UpsertLegality(card, "Pauper", IsLegal(legality.Pauper), false);
            result.MainObject.Add(pauper.MainObject);
            result.Merge(pauper);

            var pauperCmdr = UpsertLegality(card, "PauperCommander", IsLegal(legality.PauperCommander), IsLegal(legality.PauperCommander) && leadership.Commander);
            result.MainObject.Add(pauperCmdr.MainObject);
            result.Merge(pauperCmdr);

            var penny = UpsertLegality(card, "Penny", IsLegal(legality.Penny), false);
            result.MainObject.Add(penny.MainObject);
            result.Merge(penny);

            var predh = UpsertLegality(card, "PreDH", IsLegal(legality.Predh), IsLegal(legality.Predh) && leadership.Commander);
            result.MainObject.Add(predh.MainObject);
            result.Merge(predh);

            var premodern = UpsertLegality(card, "PreModern", IsLegal(legality.Premodern), false);
            result.MainObject.Add(premodern.MainObject);
            result.Merge(premodern);

            var standard = UpsertLegality(card, "Standard", IsLegal(legality.Standard), false);
            result.MainObject.Add(standard.MainObject);
            result.Merge(standard);

            var vintage = UpsertLegality(card, "Vintage", IsLegal(legality.Vintage), false);
            result.MainObject.Add(vintage.MainObject);
            result.Merge(vintage);

            var oathbreaker = UpsertLegality(card, "Oathbreaker", IsLegal(legality.Oathbreaker), leadership.Oathbreaker);
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

            var tinyLeaders = UpsertLegality(card, "TinyLeaders", IsLegal(tlLegality), tlCmdrLegality);
            result.MainObject.Add(tinyLeaders.MainObject);
            result.Merge(tinyLeaders);

            return result;
        }

        private static bool IsLegal(string text)
        {
            return text == "Legal" || text == "Restricted";
        }

        private static UpsertResult<Legality> UpsertLegality(Card card, string format, bool legal, bool leader)
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