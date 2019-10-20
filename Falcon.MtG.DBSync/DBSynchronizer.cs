namespace Falcon.MtG.DBSync
{
    using Falcon.MtG.MtgJsonModels;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    public class DBSynchronizer : IDisposable
    {
        private const string AllSetsFileName = "AllSets.json";
        private const string CardTypesFileName = "CardTypes.json";
        private const string KeywordsFileName = "Keywords.json";
        private const string MtgJsonUrl = "https://mtgjson.com/json/";
        private const string VersionFileName = "version.json";

        private readonly MTGDBContainer db;
        private bool disposedValue = false;
        private readonly LegalityHelper legalityHelper;
        private readonly string workingDirectory;

        public DBSynchronizer(string workingDir)
        {
            this.CurrentMtgJsonVersion = new JsonVersion();
            this.workingDirectory = workingDir;
            this.db = new MTGDBContainer();
            this.db.Configuration.AutoDetectChangesEnabled = false;
            this.legalityHelper = new LegalityHelper(workingDir);
        }

        public DBSynchronizer() : this(Environment.CurrentDirectory)
        {
        }

        public JsonVersion CurrentMtgJsonVersion { get; set; }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public async Task Sync(bool force)
        {
            var update = await this.DownloadJson(force);
            if (update)
            {
                var keywordsFilePath = Path.Combine(this.workingDirectory, KeywordsFileName);
                string keywordsText = await FileUtility.ReadAllTextAsync(keywordsFilePath);

                var typesFilePath = Path.Combine(this.workingDirectory, CardTypesFileName);
                string cardTypesText = await FileUtility.ReadAllTextAsync(typesFilePath);

                var setsFilePath = Path.Combine(this.workingDirectory, AllSetsFileName);
                string setsText = await FileUtility.ReadAllTextAsync(setsFilePath);

                var keywords = JsonConvert.DeserializeObject<JsonKeywords>(keywordsText);

                await this.SyncKeywords(keywords);

                dynamic parsedCardTypes = JsonConvert.DeserializeObject(cardTypesText);

                await this.SyncCardTypes(parsedCardTypes.types);

                await this.db.SeedColorData();

                await this.SaveChanges();

                JObject parsedSetData = JsonConvert.DeserializeObject<JObject>(setsText);

                var setList = new List<JsonSet>();
                foreach (var set in parsedSetData)
                {
                    setList.Add(set.Value.ToObject<JsonSet>());
                }

                setList.Sort(new JsonSetComparer());

                foreach (var set in setList)
                {
                    await this.SyncSet(set);
                }

                await this.SaveChanges();

                // Save successfully synced version number
                var versionFilePath = Path.Combine(this.workingDirectory, VersionFileName);
                File.WriteAllText(versionFilePath, JsonConvert.SerializeObject(this.CurrentMtgJsonVersion));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.db.Dispose();
                }

                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Update JSON files from MtGJson.
        /// </summary>
        /// <param name="force">If true, update regardless of version difference.</param>
        private async Task<bool> DownloadJson(bool force)
        {
            var versionFilePath = Path.Combine(this.workingDirectory, VersionFileName);
            var setsFilePath = Path.Combine(this.workingDirectory, AllSetsFileName);
            var keywordsFilePath = Path.Combine(this.workingDirectory, KeywordsFileName);
            var typesFilePath = Path.Combine(this.workingDirectory, CardTypesFileName);

            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2;)");

                if (File.Exists(versionFilePath))
                {
                    this.CurrentMtgJsonVersion = JsonConvert.DeserializeObject<JsonVersion>(File.ReadAllText(versionFilePath));
                }

                Console.WriteLine("Current Version: " + this.CurrentMtgJsonVersion.Version);

                var newVersion = JsonConvert.DeserializeObject<JsonVersion>(client.DownloadString(MtgJsonUrl + VersionFileName));
                Console.WriteLine("Server Version:  " + newVersion.Version);

                if (force || this.CurrentMtgJsonVersion != newVersion)
                {
                    this.CurrentMtgJsonVersion = newVersion;
                    Console.WriteLine("Database will be updated.");

                    await client.DownloadFileTaskAsync(MtgJsonUrl + KeywordsFileName, keywordsFilePath);
                    await client.DownloadFileTaskAsync(MtgJsonUrl + CardTypesFileName, typesFilePath);
                    await client.DownloadFileTaskAsync(MtgJsonUrl + AllSetsFileName, setsFilePath);

                    return true;
                }
            }

            Console.WriteLine("Version matches; no need to sync.");
            return false;
        }

        private async Task LinkSides(JsonCard jsonCard)
        {
            var sideAName = jsonCard.Names?.FirstOrDefault();
            if (sideAName != null && jsonCard.Side != null && jsonCard.Side != "a")
            {
                var mainSide = await db.Cards
                    .Include(c => c.OtherSides)
                    .Where(c => c.Name == sideAName)
                    .SingleOrDefaultAsync();

                var altCard = await db.Cards
                    .Include(c => c.MainSide)
                    .Where(c => c.Name == jsonCard.Name)
                    .SingleOrDefaultAsync();

                if (altCard != null)
                {
                    altCard.MainSide = mainSide;
                }
            }
        }

        private async Task<List<Keyword>> ParseKeywords(string OracleText)
        {
            // TODO: Figure out better parsing instead of just contains the name or phrase
            return await db.Keywords.Where(k => OracleText.ToLower().Contains(k.Name.ToLower())).ToListAsync();
        }

        private async Task SaveChanges()
        {
            try
            {
                this.db.ChangeTracker.DetectChanges();
                if (this.db.ChangeTracker.HasChanges())
                {
                    await this.db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                var changes = this.db.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged);
                foreach (var entity in changes)
                {
                    await entity.ReloadAsync();
                }
            }
        }

        private async Task SyncCardTypes(JObject cardTypes)
        {
            Console.WriteLine("Syncing card types...");
            foreach (var cardType in cardTypes)
            {
                await this.UpsertCardType(cardType.Key, cardType.Value.ToObject<JsonCardTypes>());
            }
        }

        private async Task SyncKeywords(JsonKeywords keywords)
        {
            Console.WriteLine("Syncing keywords...");
            foreach (var keyword in keywords.AbilityWords)
            {
                await this.UpsertKeyword(keyword, "Ability Word");
            }

            foreach (var keyword in keywords.KeywordAbilities)
            {
                await this.UpsertKeyword(keyword, "Keyword Ability");
            }

            foreach (var keyword in keywords.KeywordActions)
            {
                await this.UpsertKeyword(keyword, "Keyword Action");
            }
        }

        private async Task SyncSet(JsonSet set)
        {
            Console.WriteLine("Syncing set {0} - {1}", set.Code, set.Name);

            using (var transaction = db.Database.BeginTransaction())
            {
                await this.UpsertSet(set);
                await this.SaveChanges();

                transaction.Commit();
            }

            var multiSideCards = set.Cards.Where(c => c.Side != null && c.Side != "a").ToList();
            foreach (var card in multiSideCards)
            {
                await this.LinkSides(card);
            }

            await this.SaveChanges();
        }

        private async Task<Card> UpsertCard(JsonCard printing)
        {
            var dbCard = db.Cards.Local
            .Where(c => c.Name == printing.Name)
            .SingleOrDefault();

            if (dbCard == null)
            {
                dbCard = await db.Cards
                .Include(c => c.Colors)
                .Include(c => c.ColorIdentity)
                .Include(c => c.Supertypes)
                .Include(c => c.Types)
                .Include(c => c.Subtypes)
                .Include(c => c.Layout)
                .Include(c => c.Legalities)
                .Include(c => c.Keywords)
                .Include(c => c.MainSide)
                .Where(c => c.Name == printing.Name)
                .SingleOrDefaultAsync();
            }

            if (dbCard == null)
            {
                dbCard = db.Cards.Add(new Card()
                {
                    Name = printing.Name
                });
            }

            dbCard.ManaCost = printing.ManaCost;
            dbCard.CMC = printing.ConvertedManaCost;
            dbCard.TypeLine = printing.Type;
            dbCard.OracleText = printing.Text ?? string.Empty;
            dbCard.Power = printing.Power;
            dbCard.Toughness = printing.Toughness;
            dbCard.Loyalty = printing.Loyalty;
            dbCard.EDHRECRank = printing.EDHRECRank;

            if (dbCard.Colors.Select(t => t.Symbol).Except(printing.Colors).Any() ||
                printing.Colors.Except(dbCard.Colors.Select(t => t.Symbol)).Any())
            {
                dbCard.Colors = new List<Color>();
                dbCard.Colors = await db.Colors.Where(t => printing.Colors.Contains(t.Name)).ToListAsync();
            }

            if (dbCard.ColorIdentity.Select(t => t.Symbol).Except(printing.Colors).Any() ||
                printing.Colors.Except(dbCard.ColorIdentity.Select(t => t.Symbol)).Any())
            {
                dbCard.ColorIdentity = new List<Color>();
                dbCard.ColorIdentity = await db.Colors.Where(t => printing.Colors.Contains(t.Name)).ToListAsync();
            }

            if (dbCard.Supertypes.Select(t => t.Name).Except(printing.Supertypes).Any() ||
                printing.Supertypes.Except(dbCard.Supertypes.Select(t => t.Name)).Any())
            {
                dbCard.Supertypes = new List<Supertype>();
                dbCard.Supertypes = await db.Supertypes.Where(t => printing.Supertypes.Contains(t.Name)).ToListAsync();
            }

            if (dbCard.Types.Select(t => t.Name).Except(printing.Types).Any() ||
                printing.Types.Except(dbCard.Types.Select(t => t.Name)).Any())
            {
                dbCard.Types = new List<CardType>();
                dbCard.Types = await db.CardTypes.Where(t => printing.Types.Contains(t.Name)).ToListAsync();
            }

            if (dbCard.Subtypes.Select(t => t.Name).Except(printing.Subtypes).Any() ||
                printing.Subtypes.Except(dbCard.Subtypes.Select(t => t.Name)).Any())
            {
                dbCard.Subtypes = new List<Subtype>();
                dbCard.Subtypes = await db.Subtypes.Where(t => printing.Subtypes.Contains(t.Name)).ToListAsync();
            }

            dbCard.Layout = await UpsertSimpleLookup(db.Layouts, printing.Layout);

            dbCard.Legalities = legalityHelper.UpsertLegalities(db, dbCard, printing.Legalities);

            var keywords = (await ParseKeywords(printing.Text)).Select(k => k.Name);
            if (dbCard.Keywords.Select(t => t.Name).Except(keywords).Any() ||
                keywords.Except(dbCard.Subtypes.Select(t => t.Name)).Any())
            {
                dbCard.Keywords = new List<Keyword>();
                dbCard.Keywords = await db.Keywords.Where(t => keywords.Contains(t.Name)).ToListAsync();
            }

            dbCard.Side = printing.Side;
            if (printing.Side == null || printing.Side == "a")
            {
                dbCard.MainSide = null;
            }

            return dbCard;
        }

        private async Task<CardType> UpsertCardType(string cardType, JsonCardTypes cardTypeTypes)
        {
            var dbCardType = await db.CardTypes
                .Include(t => t.Subtypes)
                .Include(t => t.Supertypes)
                .Where(t => t.Name == cardType)
                .SingleOrDefaultAsync();

            if (dbCardType == null)
            {
                dbCardType = db.CardTypes.Add(new CardType()
                {
                    Name = cardType
                });
            }

            var subtypes = new List<Subtype>();
            foreach (var subtype in cardTypeTypes.SubTypes)
            {
                var type = await UpsertSimpleLookup(db.Subtypes, subtype);
                if (type != null)
                {
                    subtypes.Add(type);
                }
            }

            dbCardType.Subtypes = subtypes;

            var supertypes = new List<Supertype>();
            foreach (var supertype in cardTypeTypes.SuperTypes)
            {
                var type = await UpsertSimpleLookup(db.Supertypes, supertype);
                if (type != null)
                {
                    supertypes.Add(type);
                }
            }

            dbCardType.Supertypes = supertypes;

            return dbCardType;
        }

        private async Task<Keyword> UpsertKeyword(string keyword, string keywordType)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return null;
            }

            var existingKeyword = db.Keywords.Local.Where(k => k.Name == keyword).SingleOrDefault();
            if (existingKeyword == null)
            {
                existingKeyword = await db.Keywords.Where(k => k.Name == keyword).SingleOrDefaultAsync();
            }

            if (existingKeyword == null)
            {
                existingKeyword = db.Keywords.Add(new Keyword()
                {
                    Name = keyword
                });
            }

            existingKeyword.Type = keywordType;

            return existingKeyword;
        }

        private void UpsertPricing(Printing printing, JObject prices, bool foil)
        {
            var properties = prices.Properties();
            foreach (var property in properties)
            {
                if (DateTime.TryParse(property.Name, out DateTime date))
                {
                    var dbPricing = printing.Pricings.Where(p => p.Date.Date == date.Date && p.Foil == foil).SingleOrDefault();
                    if (dbPricing == null)
                    {
                        dbPricing = db.Pricings.Add(new Pricing()
                        {
                            Printing = printing,
                            Foil = foil,
                            Date = date
                        });
                    }

                    dbPricing.Price = (double)property.Value;
                }
            }
        }

        private async Task<Printing> UpsertPrinting(JsonCard printing)
        {
            var card = await this.UpsertCard(printing);

            if (!printing.MultiverseId.HasValue)
            {
                return null;
            }

            var dbPrinting = await db.Printings
                .Include(p => p.Card)
                .Include(p => p.Set)
                .Include(p => p.Artist)
                .Include(p => p.Watermark)
                .Include(p => p.Frame)
                .Include(p => p.Rarity)
                .Include(p => p.Border)
                .Include(p => p.Pricings)
                .Where(p => p.MultiverseId == printing.MultiverseId.Value && p.Side == printing.Side)
                .SingleOrDefaultAsync();

            if (dbPrinting == null)
            {
                dbPrinting = db.Printings.Add(new Printing()
                {
                    MultiverseId = printing.MultiverseId.Value,
                    Side = printing.Side
                });
            }

            dbPrinting.FlavorText = printing.FlavorText;
            dbPrinting.CollectorNumber = printing.Number;

            dbPrinting.Artist = await UpsertSimpleLookup(db.Artists, printing.Artist);
            dbPrinting.Watermark = await UpsertSimpleLookup(db.Watermarks, printing.Watermark);
            dbPrinting.Frame = await UpsertSimpleLookup(db.Frames, printing.FrameVersion);
            dbPrinting.Rarity = await UpsertSimpleLookup(db.Rarities, printing.Rarity);
            dbPrinting.Border = await UpsertSimpleLookup(db.Borders, printing.BorderColor);

            if (printing.Prices?.Paper != null)
            {
                UpsertPricing(dbPrinting, printing.Prices.Paper, false);
            }

            if (printing.Prices?.PaperFoil != null)
            {
                UpsertPricing(dbPrinting, printing.Prices.PaperFoil, true);
            }

            dbPrinting.Card = card;

            return dbPrinting;
        }

        private async Task UpsertSet(JsonSet set)
        {
            var setType = await UpsertSimpleLookup(db.SetTypes, set.Type);
            var block = await UpsertSimpleLookup(db.Blocks, set.Block);

            var dbSet = await db.Sets
                .Include(s => s.SetType)
                .Include(s => s.Block)
                .Include(s => s.Printings)
                .Where(s => s.Name == set.Name)
                .SingleOrDefaultAsync();

            if (dbSet == null)
            {
                dbSet = db.Sets.Add(new Set()
                {
                    Name = set.Name
                });
            }

            dbSet.Code = set.Code;
            dbSet.KeyruneCode = set.KeyruneCode;
            dbSet.Date = set.ReleaseDate;
            dbSet.SetType = setType;
            dbSet.Block = block;

            foreach (var card in set.Cards)
            {
                // Add missing sides property to Who/What/Where/When/Why because that card is the worst
                if (string.IsNullOrEmpty(card.Side))
                {
                    switch (card.Name)
                    {
                        case "What":
                            card.Side = "b";
                            break;
                        case "When":
                            card.Side = "c";
                            break;
                        case "Where":
                            card.Side = "d";
                            break;
                        case "Why":
                            card.Side = "e";
                            break;
                    }
                }

                dbSet.Printings.Add(await this.UpsertPrinting(card));
            }
        }

        private async Task<T> UpsertSimpleLookup<T>(DbSet<T> lookups, string name) where T : class, ISimpleLookup, new()
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var existingLookup = lookups.Local.Where(t => t.Name == name).SingleOrDefault();
            if (existingLookup == null)
            {
                existingLookup = await lookups.Where(t => t.Name == name).SingleOrDefaultAsync();
            }

            if (existingLookup == null)
            {
                existingLookup = lookups.Add(new T()
                {
                    Name = name
                });
            }

            return existingLookup;
        }
    }
}