namespace Falcon.MtG.DBSync
{
    using Falcon.MtG.MtgJsonModels;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SharpCompress.Common;
    using SharpCompress.Readers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    public class DBSynchronizer : IDisposable
    {
        private const string AllSetsArchiveFileName = "AllSetFiles.tar.bz2";
        private const string SetsFolderName = "Sets";
        private const string SetListFileName = "SetList.json";
        private const string CardTypesFileName = "CardTypes.json";
        private const string KeywordsFileName = "Keywords.json";
        private const string MtgJsonUrl = "https://mtgjson.com/json/";
        private const string VersionFileName = "version.json";

        private MtGDBContext db;
        private bool disposedValue = false;
        private readonly LegalityHelper legalityHelper;
        private readonly string workingDirectory;

        public DBSynchronizer(string workingDir)
        {
            this.CurrentMtgJsonVersion = new JsonVersion();
            this.workingDirectory = workingDir;
            this.legalityHelper = new LegalityHelper();
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
                await this.RefreshContext();

                //await this.SyncKeywords();
                await this.SyncCardTypes();
                this.db.SeedColorData();
                await this.SaveChanges();

                var setList = await this.LoadSetList();
                foreach (var setCode in setList)
                {
                    await this.RefreshContext();
                    var loadedSet = await this.LoadSet(setCode);
                    //await db.LoadLookups();
                    await this.SyncSet(loadedSet);
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

        protected async Task RefreshContext()
        {
            if (this.db != null)
            {
                await this.db.DisposeAsync();
            }

            this.db = new MtGDBContext();
            this.db.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        private async Task<JsonSet> LoadSet(string setCode)
        {
            // Conflux's file name has to be CON_ to avoid OS issues.
            var jsonFileName = setCode + (setCode == "CON" ? "_" : string.Empty) + ".json";
            var setDataPath = Path.Combine(this.workingDirectory, SetsFolderName, jsonFileName);
            var setData = await File.ReadAllTextAsync(setDataPath);
            var set = JsonConvert.DeserializeObject<JsonSet>(setData);

            await db.Sets
                .Include(s => s.Block)
                .Include(s => s.SetType)
                .Where(s => s.Code == setCode).LoadAsync();

            await db.Printings
                //.Include(p => p.Card)
                //.Include(p => p.Set)
                .Include(p => p.Artist)
                .Include(p => p.Watermark)
                .Include(p => p.Frame)
                .Include(p => p.Rarity)
                .Include(p => p.Border)
                .Include(p => p.Pricings)
                .Where(p => set.Cards.Select(c => c.MultiverseId).Contains(p.MultiverseId))
                .LoadAsync();

            await db.Cards
                .Include(c => c.Colors)
                    .ThenInclude(c => c.Color)
                .Include(c => c.ColorIdentity)
                    .ThenInclude(c => c.Color)
                .Include(c => c.Supertypes)
                    .ThenInclude(t => t.Supertype)
                .Include(c => c.Types)
                    .ThenInclude(t => t.CardType)
                .Include(c => c.Subtypes)
                    .ThenInclude(t => t.Subtype)
                .Include(c => c.Layout)
                .Include(c => c.Legalities)
                .Include(c => c.Keywords)
                .Include(c => c.MainSide)
                .Where(c => set.Cards.Select(sc => sc.Name).Contains(c.Name))
                .LoadAsync();

            return set;
        }

        private async Task<IEnumerable<string>> LoadSetList()
        {
            var setsArchiveFilePath = Path.Combine(this.workingDirectory, AllSetsArchiveFileName);
            var setsSubfolderPath = Path.Combine(this.workingDirectory, SetsFolderName);
            var extractionOptions = new ExtractionOptions()
            {
                ExtractFullPath = true,
                Overwrite = true
            };

            using (var stream = File.OpenRead(setsArchiveFilePath))
            {
                using var reader = ReaderFactory.Open(stream);
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        reader.WriteEntryToDirectory(setsSubfolderPath, extractionOptions);
                    }
                }
            }

            string setsText = await FileUtility.ReadAllTextAsync(SetListFileName);
            JArray parsedSetData = JsonConvert.DeserializeObject<JArray>(setsText);

            var setList = new List<JsonSet>();
            foreach (var set in parsedSetData)
            {
                setList.Add(set.ToObject<JsonSet>());
            }

            setList.Sort(new JsonSetComparer());

            return setList.Select(s => s.Code);
        }

        /// <summary>
        /// Update JSON files from MtGJson.
        /// </summary>
        /// <param name="force">If true, update regardless of version difference.</param>
        private async Task<bool> DownloadJson(bool force)
        {
            var versionFilePath = Path.Combine(this.workingDirectory, VersionFileName);
            var setsFilePath = Path.Combine(this.workingDirectory, AllSetsArchiveFileName);
            var setListFilePath = Path.Combine(this.workingDirectory, SetListFileName);
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
                    await client.DownloadFileTaskAsync(MtgJsonUrl + AllSetsArchiveFileName, setsFilePath);
                    await client.DownloadFileTaskAsync(MtgJsonUrl + SetListFileName, setListFilePath);

                    return true;
                }
            }

            Console.WriteLine("Version matches; no need to sync.");
            return false;
        }

        private void LinkSides(JsonCard jsonCard)
        {
            var sideAName = jsonCard.Names?.FirstOrDefault();
            if (sideAName != null && jsonCard.Side != null && jsonCard.Side != "a")
            {
                var mainSide = db.Cards.Local
                    .Where(c => c.Name == sideAName)
                    .FirstOrDefault();

                var altCard = db.Cards.Local
                    .Where(c => c.Name == jsonCard.Name)
                    .FirstOrDefault();

                if (altCard != null)
                {
                    altCard.MainSide = mainSide;
                }
            }
        }

        private async Task<List<Keyword>> ParseKeywords(string OracleText)
        {
            // TODO: Figure out better parsing instead of just contains the name or phrase
            if (string.IsNullOrWhiteSpace(OracleText))
            {
                return new List<Keyword>();
            }

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
                await this.RefreshContext();
            }
        }

        private async Task SyncCardTypes()
        {
            Console.WriteLine("Syncing card types...");
            var typesFilePath = Path.Combine(this.workingDirectory, CardTypesFileName);
            string cardTypesText = await FileUtility.ReadAllTextAsync(typesFilePath);
            dynamic parsedCardTypes = JsonConvert.DeserializeObject(cardTypesText);

            JObject types = parsedCardTypes.types;
            foreach (var cardType in types)
            {
                await this.UpsertCardType(cardType.Key, cardType.Value.ToObject<JsonCardTypes>());
            }
        }

        private async Task SyncKeywords()
        {
            Console.WriteLine("Syncing keywords...");

            var keywordsFilePath = Path.Combine(this.workingDirectory, KeywordsFileName);
            string keywordsText = await FileUtility.ReadAllTextAsync(keywordsFilePath);
            var keywords = JsonConvert.DeserializeObject<JsonKeywords>(keywordsText);

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

            var setUpsert = await this.UpsertSet(set);

            db.RemoveRange(setUpsert.ObjectsToRemove);
            db.AddRange(setUpsert.ObjectsToAdd);

            var multiSideCards = set.Cards.Where(c => c.Side != null && c.Side != "a");
            foreach (var card in multiSideCards)
            {
                this.LinkSides(card);
            }

            await this.SaveChanges();
        }

        private async Task<UpsertResult<Card>> UpsertCard(JsonCard printing)
        {
            var result = new UpsertResult<Card>();

            var dbCard = db.Cards.Local
            .Where(c => c.Name == printing.Name)
            .FirstOrDefault();

            if (dbCard == null)
            {
                dbCard = new Card()
                {
                    Name = printing.Name
                };

                result.ObjectsToAdd.Add(dbCard);
                //db.Cards.Local.Add(dbCard);
            }

            dbCard.ManaCost = printing.ManaCost;
            dbCard.CMC = printing.ConvertedManaCost;
            dbCard.TypeLine = printing.Type;
            dbCard.OracleText = printing.Text ?? string.Empty;
            dbCard.Power = printing.Power;
            dbCard.Toughness = printing.Toughness;
            dbCard.Loyalty = printing.Loyalty;
            dbCard.EDHRECRank = printing.EDHRECRank;

            printing.Types = printing.Types.ConvertAll(t => t.ToLower());

            if (dbCard.Colors.Select(t => t.Color.Symbol).Except(printing.Colors).Any() ||
                printing.Colors.Except(dbCard.Colors.Select(t => t.Color.Symbol)).Any())
            {
                result.ObjectsToRemove.AddRange(dbCard.Colors);

                var colors = db.Colors.Where(c => printing.Colors.Contains(c.Name));
                var cardColors = new List<CardColor>();
                foreach(var color in colors)
                {
                    cardColors.Add(new CardColor()
                    {
                         Card = dbCard,
                         Color = color
                    });
                }

                result.ObjectsToAdd.AddRange(cardColors);
            }

            if (dbCard.ColorIdentity.Select(t => t.Color.Symbol).Except(printing.Colors).Any() ||
                printing.Colors.Except(dbCard.ColorIdentity.Select(t => t.Color.Symbol)).Any())
            {
                result.ObjectsToRemove.AddRange(dbCard.ColorIdentity);

                var colors = db.Colors.Where(c => printing.ColorIdentity.Contains(c.Name));
                var cardColorIdentity = new List<CardColorIdentity>();
                foreach (var color in colors)
                {
                    cardColorIdentity.Add(new CardColorIdentity()
                    {
                        Card = dbCard,
                        Color = color
                    });
                }

                result.ObjectsToAdd.AddRange(cardColorIdentity);
            }

            if (dbCard.Supertypes.Select(t => t.Supertype.Name).Except(printing.Supertypes).Any() ||
                printing.Supertypes.Except(dbCard.Supertypes.Select(t => t.Supertype.Name)).Any())
            {
                result.ObjectsToRemove.AddRange(dbCard.Supertypes);

                var supertypes = db.Supertypes.Where(t => printing.Supertypes.Contains(t.Name));
                var cardSupertypes = new List<CardSupertype>();
                foreach (var supertype in supertypes)
                {
                    cardSupertypes.Add(new CardSupertype()
                    {
                        Card = dbCard,
                        Supertype = supertype
                    });
                }

                result.ObjectsToAdd.AddRange(cardSupertypes);
            }

            if (dbCard.Types.Select(t => t.CardType.Name).Except(printing.Types).Any() ||
                printing.Types.Except(dbCard.Types.Select(t => t.CardType.Name)).Any())
            {
                result.ObjectsToRemove.AddRange(dbCard.Types);

                var cardTypes = db.CardTypes.Where(t => printing.Types.Contains(t.Name));
                var cardCardTypes = new List<CardCardType>();
                foreach (var cardType in cardTypes)
                {
                    cardCardTypes.Add(new CardCardType()
                    {
                        Card = dbCard,
                        CardType = cardType
                    });
                }

                result.ObjectsToAdd.AddRange(cardCardTypes);
            }

            if (dbCard.Subtypes.Select(t => t.Subtype.Name).Except(printing.Subtypes).Any() ||
                printing.Subtypes.Except(dbCard.Subtypes.Select(t => t.Subtype.Name)).Any())
            {
                result.ObjectsToRemove.AddRange(dbCard.Subtypes);

                var subtypes = db.Subtypes.Where(t => printing.Subtypes.Contains(t.Name));
                var cardSubtypes = new List<CardSubtype>();
                foreach (var subtype in subtypes)
                {
                    cardSubtypes.Add(new CardSubtype()
                    {
                        Card = dbCard,
                        Subtype = subtype
                    });
                }

                result.ObjectsToAdd.AddRange(cardSubtypes);
            }

            dbCard.Layout = await UpsertSimpleLookup(db.Layouts, printing.Layout);

            var legalityUpsert = legalityHelper.UpsertLegalities(dbCard, printing.Legalities, printing.LeadershipSkills);
            dbCard.Legalities = legalityUpsert.MainObject;
            result.Merge(legalityUpsert);

            //var keywords = (await ParseKeywords(printing.Text)).Select(k => k.Name);
            //if (dbCard.Keywords.Select(t => t.Keyword.Name).Except(keywords).Any() ||
            //    keywords.Except(dbCard.Keywords.Select(t => t.Keyword.Name)).Any())
            //{
            //    if (dbCard.Keywords.Count > 0)
            //    {
            //        await this.SaveChanges();
            //        db.CardKeywords.RemoveRange(dbCard.Keywords);
            //    }

            //    var dbKeywords = await db.Keywords.Where(k => keywords.Contains(k.Name)).ToListAsync();
            //    var cardKeywords = new List<CardKeyword>();
            //    foreach (var keyword in dbKeywords)
            //    {
            //        cardKeywords.Add(new CardKeyword()
            //        {
            //            Card = dbCard,
            //            Keyword = keyword
            //        });
            //    }

            //    db.CardKeywords.AddRange(cardKeywords);
            //}

            dbCard.Side = printing.Side;
            if (printing.Side == null || printing.Side == "a")
            {
                dbCard.MainSide = null;
            }

            result.MainObject = dbCard;

            return result;
        }

        private async Task<CardType> UpsertCardType(string cardType, JsonCardTypes cardTypeTypes)
        {
            var dbCardType = await db.CardTypes
                .Include(t => t.Subtypes)
                    .ThenInclude(t => t.Subtype)
                .Include(t => t.Supertypes)
                    .ThenInclude(t => t.Supertype)
                .Where(t => t.Name == cardType)
                .FirstOrDefaultAsync();

            if (dbCardType == null)
            {
                dbCardType = new CardType()
                {
                    Name = cardType
                };
                
                db.CardTypes.Add(dbCardType);
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

            if (dbCardType.Subtypes.Select(t => t.Subtype.Name).Except(cardTypeTypes.SubTypes).Any() ||
                cardTypeTypes.SubTypes.Except(dbCardType.Subtypes.Select(t => t.Subtype.Name)).Any())
            {
                db.CardTypeSubtypes.RemoveRange(dbCardType.Subtypes);

                var cardTypeSubtypes = new List<CardTypeSubtype>();
                foreach (var subtype in subtypes)
                {
                    cardTypeSubtypes.Add(new CardTypeSubtype()
                    {
                        CardType = dbCardType,
                        Subtype = subtype
                    });
                }

                db.CardTypeSubtypes.AddRange(cardTypeSubtypes);
            }

            var supertypes = new List<Supertype>();
            foreach (var supertype in cardTypeTypes.SuperTypes)
            {
                var type = await UpsertSimpleLookup(db.Supertypes, supertype);
                if (type != null)
                {
                    supertypes.Add(type);
                }
            }

            if (dbCardType.Supertypes.Select(t => t.Supertype.Name).Except(cardTypeTypes.SubTypes).Any() ||
                cardTypeTypes.SubTypes.Except(dbCardType.Supertypes.Select(t => t.Supertype.Name)).Any())
            {
                db.CardTypeSupertypes.RemoveRange(dbCardType.Supertypes);

                var cardTypeSupertypes = new List<CardTypeSupertype>();
                foreach (var supertype in supertypes)
                {
                    cardTypeSupertypes.Add(new CardTypeSupertype()
                    {
                        CardType = dbCardType,
                        Supertype = supertype
                    });
                }

                db.CardTypeSupertypes.AddRange(cardTypeSupertypes);
            }

            return dbCardType;
        }

        private async Task<Keyword> UpsertKeyword(string keyword, string keywordType)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return null;
            }

            var existingKeyword = await db.Keywords.Where(k => k.Name == keyword).FirstOrDefaultAsync();
            if (existingKeyword == null)
            {
                existingKeyword = new Keyword()
                {
                    Name = keyword
                };
                
                db.Keywords.Add(existingKeyword);
            }

            existingKeyword.Type = keywordType;

            return existingKeyword;
        }

        private UpsertResult<Pricing> UpsertPricing(Printing printing, JObject prices, bool foil)
        {
            var result = new UpsertResult<Pricing>();

            var properties = prices.Properties();
            foreach (var property in properties)
            {
                if (DateTime.TryParse(property.Name, out DateTime date))
                {
                    var dbPricing = printing.Pricings.Where(p => p.Date.Date == date.Date && p.Foil == foil).FirstOrDefault();
                    if (dbPricing == null)
                    {
                        dbPricing = new Pricing()
                        {
                            Printing = printing,
                            Foil = foil,
                            Date = date
                        };

                        //db.Pricings.Add(dbPricing);
                        result.ObjectsToAdd.Add(dbPricing);
                    }

                    dbPricing.Price = (double)property.Value;
                }
            }

            return result;
        }

        private async Task<UpsertResult<Printing>> UpsertPrinting(JsonCard printing)
        {
            var card = await this.UpsertCard(printing);

            if (card.ObjectsToRemove.Count > 0)
            {
                db.RemoveRange(card.ObjectsToRemove);
            }

            if (card.ObjectsToAdd.Count > 0)
            {
                db.AddRange(card.ObjectsToAdd);
            }

            var result = new UpsertResult<Printing>();

            if (!printing.MultiverseId.HasValue)
            {
                return result;
            }

            var dbPrinting = db.Printings.Local
                .Where(p => p.MultiverseId == printing.MultiverseId.Value && p.Side == printing.Side)
                .FirstOrDefault();

            if (dbPrinting == null)
            {
                dbPrinting = new Printing()
                {
                    MultiverseId = printing.MultiverseId.Value,
                    Side = printing.Side
                };

                result.ObjectsToAdd.Add(dbPrinting);
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
                var pricingUpsert = UpsertPricing(dbPrinting, printing.Prices.Paper, false);
                result.Merge(pricingUpsert);
            }

            if (printing.Prices?.PaperFoil != null)
            {
                var pricingUpsert = UpsertPricing(dbPrinting, printing.Prices.PaperFoil, true);
                result.Merge(pricingUpsert);
            }

            dbPrinting.Card = card.MainObject;

            result.MainObject = dbPrinting;

            return result;
        }

        private async Task<UpsertResult<Set>> UpsertSet(JsonSet set)
        {
            var result = new UpsertResult<Set>();

            var dbSet = db.Sets.Local
                .Where(s => s.Name == set.Name)
                .FirstOrDefault();

            if (dbSet == null)
            {
                dbSet = new Set()
                {
                    Name = set.Name
                };

                result.ObjectsToAdd.Add(dbSet);
            }

            dbSet.Code = set.Code;
            dbSet.KeyruneCode = set.KeyruneCode;
            dbSet.Date = set.ReleaseDate;

            dbSet.SetType = await UpsertSimpleLookup(db.SetTypes, set.Type);
            dbSet.Block = await UpsertSimpleLookup(db.Blocks, set.Block);

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

                var printingUpsert = await this.UpsertPrinting(card);
                result.Merge(printingUpsert);
                if (printingUpsert.MainObject != null)
                {
                    dbSet.Printings.Add(printingUpsert.MainObject);
                }
            }

            result.MainObject = dbSet;

            return result;
        }

        private async Task<T> UpsertSimpleLookup<T>(DbSet<T> lookups, string name) where T : class, ISimpleLookup, new()
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var existingLookup = await lookups.Where(t => t.Name == name).FirstOrDefaultAsync();

            if (existingLookup == null)
            {
                existingLookup = new T()
                {
                    Name = name
                };

                lookups.Add(existingLookup);
            }

            return existingLookup;
        }
    }
}