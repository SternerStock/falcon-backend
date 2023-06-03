namespace Falcon.MtG.DBSync
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Falcon.MtG.Models.Json;
    using Falcon.MtG.Models.Sql;
    using Falcon.MtG.Utility;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SharpCompress.Common;
    using SharpCompress.Readers;

    public class DBSynchronizer : IDisposable
    {
        private const string AllSetsArchiveFileName = "AllSetFiles.tar.bz2";
        private const string SetsFolderName = "AllSetFiles";
        private const string SetListFileName = "SetList.json";
        private const string CardTypesFileName = "CardTypes.json";
        private const string KeywordsFileName = "Keywords.json";
        private const string MtgJsonUrl = "https://mtgjson.com/api/v5/";
        private const string VersionFileName = "Meta.json";

        private MtGDBContext _db;
        private readonly HttpClient _client;
        private bool disposedValue = false;
        private readonly LegalityHelper _legalityHelper;
        private readonly string _workingDirectory;

        public DBSynchronizer(string workingDir)
        {
            this._client = new HttpClient();
            this.CurrentMtgJsonVersion = new JsonVersion();
            this._workingDirectory = workingDir;
            this._legalityHelper = new LegalityHelper();
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

                await this.SyncKeywords();
                await this.SyncCardTypes();
                this._db.SeedColorData();
                await this.SaveChanges();

                var setList = await this.LoadSetList();
                foreach (var setCode in setList)
                {
                    await this.RefreshContext();
                    var loadedSet = await this.LoadSet(setCode);
                    await _db.LoadLookups();
                    await this.SyncSet(loadedSet);
                }

                await this.SaveChanges();

                // Save successfully synced version number
                var versionFilePath = Path.Combine(this._workingDirectory, VersionFileName);
                File.WriteAllText(versionFilePath, JsonConvert.SerializeObject(this.CurrentMtgJsonVersion));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this._db.Dispose();
                }

                this.disposedValue = true;
            }
        }

        protected async Task RefreshContext()
        {
            if (this._db != null)
            {
                await this._db.DisposeAsync();
            }

            this._db = new MtGDBContext();
            this._db.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        private async Task<JsonSet> LoadSet(string setCode)
        {
            // Conflux's file name has to be CON_ to avoid OS issues.
            var jsonFileName = setCode + (setCode == "CON" ? "_" : string.Empty) + ".json";
            var setDataPath = Path.Combine(this._workingDirectory, SetsFolderName, jsonFileName);
            var setData = await File.ReadAllTextAsync(setDataPath);
            var set = Utility.ParseMtGJson<JsonSet>(setData);

            await _db.Sets
                .Include(s => s.Block)
                .Include(s => s.SetType)
                .Where(s => s.Code == setCode).LoadAsync();

            await _db.Printings
                //.Include(p => p.Card)
                //.Include(p => p.Set)
                .Include(p => p.Artist)
                .Include(p => p.Watermark)
                .Include(p => p.Frame)
                .Include(p => p.Rarity)
                .Include(p => p.Border)
                .Include(p => p.Pricings)
                .Where(p => set.Cards.Select(c => c.Identifiers.MultiverseId).Contains(p.MultiverseId))
                .LoadAsync();

            await _db.Cards
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
                .Where(c => set.Cards.Select(sc => sc.CockatriceName).Contains(c.Name))
                .LoadAsync();

            return set;
        }

        private async Task<IEnumerable<string>> LoadSetList()
        {
            string setsText = await FileUtility.ReadAllTextAsync(SetListFileName);
            JArray parsedSetData = Utility.ParseMtGJson<JArray>(setsText);

            var setList = new List<JsonSet>();
            foreach (var set in parsedSetData)
            {
                setList.Add(set.ToObject<JsonSet>());
            }

            setList.Sort(new JsonSetComparer());

            return setList.Select(s => s.Code);
        }

        private async Task DownloadAndExtractFile(string fileName)
        {
            var extractionOptions = new ExtractionOptions()
            {
                ExtractFullPath = true,
                Overwrite = true
            };

            string downloadUrl = $"{MtgJsonUrl}{fileName}{(fileName.EndsWith(".json") ? ".zip" : string.Empty)}";

            using var response = await _client.GetAsync(downloadUrl);
            using var downloadStream = await response.Content.ReadAsStreamAsync();
            downloadStream.Position = 0;

            if (downloadUrl.EndsWith(".bz2") ||
                downloadUrl.EndsWith(".gz") ||
                downloadUrl.EndsWith(".xz") ||
                downloadUrl.EndsWith(".zip"))
            {
                using var reader = ReaderFactory.Open(downloadStream);
                while (reader.MoveToNextEntry())
                {
                    reader.WriteEntryToDirectory(this._workingDirectory, extractionOptions);
                }
            }
            else
            {
                var path = Path.Combine(this._workingDirectory, fileName);
                using var fs = new FileStream(path, FileMode.OpenOrCreate);
                await downloadStream.CopyToAsync(fs);
            }
        }

        /// <summary>
        /// Update JSON files from MtGJson.
        /// </summary>
        /// <param name="force">If true, update regardless of version difference.</param>
        private async Task<bool> DownloadJson(bool force)
        {
            var versionFilePath = Path.Combine(this._workingDirectory, VersionFileName);
            if (File.Exists(versionFilePath))
            {
                string metaFileText = await File.ReadAllTextAsync(versionFilePath);
                this.CurrentMtgJsonVersion = Utility.ParseMtGJson<JsonVersion>(metaFileText);
            }

            Console.WriteLine($"Current Version: {this.CurrentMtgJsonVersion.Version}");

            var versionFileContent = await _client.GetStringAsync($"{MtgJsonUrl}{VersionFileName}");
            var newVersion = Utility.ParseMtGJson<JsonVersion>(versionFileContent);
            Console.WriteLine("Server Version:  " + newVersion.Version);

            if (force || this.CurrentMtgJsonVersion.Version != newVersion.Version)
            {
                this.CurrentMtgJsonVersion = newVersion;
                Console.WriteLine("Database will be updated.");

                List<Task> saveTasks = new()
                {
                    DownloadAndExtractFile(AllSetsArchiveFileName),
                    DownloadAndExtractFile(SetListFileName),
                    DownloadAndExtractFile(KeywordsFileName),
                    DownloadAndExtractFile(CardTypesFileName),
                    File.WriteAllTextAsync(versionFilePath, versionFileContent)
                };

                await Task.WhenAll(saveTasks);

                return true;
            }

            Console.WriteLine("Version matches; no need to sync.");
            return false;
        }

        private void LinkSides(JsonCard jsonCard)
        {
            Guid? sideAUuid = jsonCard.OtherFaceIds?.FirstOrDefault();
            if (sideAUuid != null && sideAUuid != default(Guid) && jsonCard.Side != null && jsonCard.Side != "a")
            {
                var mainSide = _db.Printings
                    .Where(p => p.UUID == sideAUuid)
                    .OrderBy(p => p.MultiverseId)
                    .FirstOrDefault()?.Card;

                var altCard = _db.Cards.Local
                    .Where(c => c.Name == jsonCard.CockatriceName)
                    .Single();

                altCard.MainSide = mainSide;
            }
        }

        private async Task SaveChanges()
        {
            try
            {
                this._db.ChangeTracker.DetectChanges();
                if (this._db.ChangeTracker.HasChanges())
                {
                    await this._db.SaveChangesAsync();
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
            var typesFilePath = Path.Combine(this._workingDirectory, CardTypesFileName);
            string cardTypesText = await FileUtility.ReadAllTextAsync(typesFilePath);
            JObject parsedCardTypes = Utility.ParseMtGJson<JObject>(cardTypesText);

            foreach (var cardType in parsedCardTypes)
            {
                await this.UpsertCardType(cardType.Key, cardType.Value.ToObject<JsonCardTypes>());
            }
        }

        private async Task SyncKeywords()
        {
            Console.WriteLine("Syncing keywords...");

            var keywordsFilePath = Path.Combine(this._workingDirectory, KeywordsFileName);
            string keywordsText = await FileUtility.ReadAllTextAsync(keywordsFilePath);
            var keywords = Utility.ParseMtGJson<JsonKeywords>(keywordsText);

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

            var artists = set.Cards.Select(c => c.Artist).Distinct();
            var watermarks = set.Cards.Select(c => c.Watermark).Distinct();
            var frames = set.Cards.Select(c => c.FrameVersion).Distinct();
            var rarities = set.Cards.Select(c => c.Rarity).Distinct();
            var borders = set.Cards.Select(c => c.BorderColor).Distinct();
            var layouts = set.Cards.Select(c => c.Layout).Distinct();

            foreach (var artist in artists)
            {
                UpsertSimpleLookup(_db.Artists, artist);
            }

            foreach (var watermark in watermarks)
            {
                UpsertSimpleLookup(_db.Watermarks, watermark);
            }

            foreach (var frame in frames)
            {
                UpsertSimpleLookup(_db.Frames, frame);
            }

            foreach (var rarity in rarities)
            {
                UpsertSimpleLookup(_db.Rarities, rarity);
            }

            foreach (var border in borders)
            {
                UpsertSimpleLookup(_db.Borders, border);
            }

            foreach (var layout in layouts)
            {
                UpsertSimpleLookup(_db.Layouts, layout);
            }

            await this.SaveChanges();

            var setUpsert = this.UpsertSet(set);

            _db.RemoveRange(setUpsert.ObjectsToRemove);
            _db.AddRange(setUpsert.ObjectsToAdd);

            var multiSideCards = set.Cards.Where(c => c.Side != null && c.Side != "a");
            foreach (var card in multiSideCards)
            {
                this.LinkSides(card);
            }

            await this.SaveChanges();
        }

        private UpsertResult<Card> UpsertCard(JsonCard printing)
        {
            var result = new UpsertResult<Card>();

            var dbCard = _db.Cards.Local
            .Where(c => c.Name == printing.CockatriceName)
            .SingleOrDefault();

            if (dbCard == null)
            {
                dbCard = new Card()
                {
                    Name = printing.CockatriceName
                };

                result.ObjectsToAdd.Add(dbCard);
            }

            dbCard.CockatriceName = printing.Name;
            dbCard.ManaCost = printing.ManaCost;
            dbCard.CMC = printing.ManaValue;
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

                var colors = _db.Colors.Where(c => printing.Colors.Contains(c.Symbol));
                var cardColors = new List<CardColor>();
                foreach (var color in colors)
                {
                    cardColors.Add(new CardColor()
                    {
                        Card = dbCard,
                        Color = color
                    });
                }

                result.ObjectsToAdd.AddRange(cardColors);
            }

            if (printing.Types.Contains("land") && !printing.Supertypes.Contains("Basic"))
            {
                if (printing.Text.Contains("plains", StringComparison.CurrentCultureIgnoreCase) && !printing.ColorIdentity.Contains("W"))
                {
                    printing.ColorIdentity.Add("W");
                }

                if (printing.Text.Contains("island", StringComparison.CurrentCultureIgnoreCase) && !printing.ColorIdentity.Contains("U"))
                {
                    printing.ColorIdentity.Add("U");
                }

                if (printing.Text.Contains("swamp", StringComparison.CurrentCultureIgnoreCase) && !printing.ColorIdentity.Contains("B"))
                {
                    printing.ColorIdentity.Add("B");
                }

                if (printing.Text.Contains("mountain", StringComparison.CurrentCultureIgnoreCase) && !printing.ColorIdentity.Contains("R"))
                {
                    printing.ColorIdentity.Add("R");
                }

                if (printing.Text.Contains("forest", StringComparison.CurrentCultureIgnoreCase) && !printing.ColorIdentity.Contains("G"))
                {
                    printing.ColorIdentity.Add("G");
                }
            }

            if (dbCard.ColorIdentity.Select(t => t.Color.Symbol).Except(printing.ColorIdentity).Any() ||
                printing.ColorIdentity.Except(dbCard.ColorIdentity.Select(t => t.Color.Symbol)).Any())
            {
                result.ObjectsToRemove.AddRange(dbCard.ColorIdentity);

                var colors = _db.Colors.Where(c => printing.ColorIdentity.Contains(c.Symbol));
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

                var supertypes = _db.Supertypes.Where(t => printing.Supertypes.Contains(t.Name));
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

                var cardTypes = _db.CardTypes.Where(t => printing.Types.Contains(t.Name));
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

                var subtypes = _db.Subtypes.Where(t => printing.Subtypes.Contains(t.Name));
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

            var keywords = printing.Keywords.Select(k => k.ToLower());
            if (dbCard.Keywords.Select(k => k.Keyword.Name.ToLower()).Except(keywords).Any() ||
                keywords.Except(dbCard.Keywords.Select(k => k.Keyword.Name.ToLower())).Any())
            {
                result.ObjectsToRemove.AddRange(dbCard.Keywords);

                var dbKeywords = _db.Keywords.Where(k => printing.Keywords.Contains(k.Name));
                var cardKeywords = new List<CardKeyword>();
                foreach (var keyword in dbKeywords)
                {
                    cardKeywords.Add(new CardKeyword()
                    {
                        Card = dbCard,
                        Keyword = keyword
                    });
                }

                result.ObjectsToAdd.AddRange(cardKeywords);
            }

            dbCard.Layout = UpsertSimpleLookup(_db.Layouts, printing.Layout);

            var legalityUpsert = _legalityHelper.UpsertLegalities(dbCard, printing.Legalities, printing.LeadershipSkills);
            dbCard.Legalities = legalityUpsert.MainObject;
            result.Merge(legalityUpsert);

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
            var dbCardType = await _db.CardTypes
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

                _db.CardTypes.Add(dbCardType);
            }

            var subtypes = new List<Subtype>();
            foreach (var subtype in cardTypeTypes.SubTypes)
            {
                var type = UpsertSimpleLookup(_db.Subtypes, subtype);
                if (type != null)
                {
                    subtypes.Add(type);
                }
            }

            if (dbCardType.Subtypes.Select(t => t.Subtype.Name).Except(cardTypeTypes.SubTypes).Any() ||
                cardTypeTypes.SubTypes.Except(dbCardType.Subtypes.Select(t => t.Subtype.Name)).Any())
            {
                _db.CardTypeSubtypes.RemoveRange(dbCardType.Subtypes);

                var cardTypeSubtypes = new List<CardTypeSubtype>();
                foreach (var subtype in subtypes)
                {
                    cardTypeSubtypes.Add(new CardTypeSubtype()
                    {
                        CardType = dbCardType,
                        Subtype = subtype
                    });
                }

                _db.CardTypeSubtypes.AddRange(cardTypeSubtypes);
            }

            var supertypes = new List<Supertype>();
            foreach (var supertype in cardTypeTypes.SuperTypes)
            {
                var type = UpsertSimpleLookup(_db.Supertypes, supertype);
                if (type != null)
                {
                    supertypes.Add(type);
                }
            }

            if (dbCardType.Supertypes.Select(t => t.Supertype.Name).Except(cardTypeTypes.SubTypes).Any() ||
                cardTypeTypes.SubTypes.Except(dbCardType.Supertypes.Select(t => t.Supertype.Name)).Any())
            {
                _db.CardTypeSupertypes.RemoveRange(dbCardType.Supertypes);

                var cardTypeSupertypes = new List<CardTypeSupertype>();
                foreach (var supertype in supertypes)
                {
                    cardTypeSupertypes.Add(new CardTypeSupertype()
                    {
                        CardType = dbCardType,
                        Supertype = supertype
                    });
                }

                _db.CardTypeSupertypes.AddRange(cardTypeSupertypes);
            }

            return dbCardType;
        }

        private async Task<Keyword> UpsertKeyword(string keyword, string keywordType)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return null;
            }

            var existingKeyword = await _db.Keywords.Where(k => k.Name == keyword).FirstOrDefaultAsync();
            if (existingKeyword == null)
            {
                existingKeyword = new Keyword()
                {
                    Name = keyword
                };

                _db.Keywords.Add(existingKeyword);
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

                    dbPricing.Price = property.Value.HasValues ? (double)property.Value : 0;
                }
            }

            return result;
        }

        private UpsertResult<Printing> UpsertPrinting(JsonCard printing)
        {
            var card = this.UpsertCard(printing);

            if (card.ObjectsToRemove.Count > 0)
            {
                _db.RemoveRange(card.ObjectsToRemove);
            }

            if (card.ObjectsToAdd.Count > 0)
            {
                _db.AddRange(card.ObjectsToAdd);
            }

            var result = new UpsertResult<Printing>();

            if (!printing.Identifiers.MultiverseId.HasValue)
            {
                return result;
            }

            var dbPrinting = _db.Printings.Local
                .Where(p => p.MultiverseId == printing.Identifiers.MultiverseId.Value && p.Side == printing.Side)
                .FirstOrDefault();

            if (dbPrinting == null)
            {
                dbPrinting = new Printing()
                {
                    MultiverseId = printing.Identifiers.MultiverseId.Value,
                    Side = printing.Side
                };

                result.ObjectsToAdd.Add(dbPrinting);
            }

            dbPrinting.UUID = printing.UUID;
            dbPrinting.FlavorText = printing.FlavorText;
            dbPrinting.CollectorNumber = printing.Number;

            dbPrinting.Artist = UpsertSimpleLookup(_db.Artists, printing.Artist);
            dbPrinting.Watermark = UpsertSimpleLookup(_db.Watermarks, printing.Watermark);
            dbPrinting.Frame = UpsertSimpleLookup(_db.Frames, printing.FrameVersion);  
            dbPrinting.Rarity = UpsertSimpleLookup(_db.Rarities, printing.Rarity);
            dbPrinting.Border = UpsertSimpleLookup(_db.Borders, printing.BorderColor);

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

        private UpsertResult<Set> UpsertSet(JsonSet set)
        {
            var result = new UpsertResult<Set>();

            var dbSet = _db.Sets.Local
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

            dbSet.SetType = UpsertSimpleLookup(_db.SetTypes, set.Type);
            dbSet.Block = UpsertSimpleLookup(_db.Blocks, set.Block);

            foreach (var card in set.Cards)
            {
                var printingUpsert = this.UpsertPrinting(card);
                result.Merge(printingUpsert);
                if (printingUpsert.MainObject != null)
                {
                    dbSet.Printings.Add(printingUpsert.MainObject);
                }
            }

            result.MainObject = dbSet;

            return result;
        }

        private T UpsertSimpleLookup<T>(DbSet<T> lookups, string name) where T : class, ISimpleLookup, new()
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var existingLookup = lookups.Local.FirstOrDefault(t => t.Name == name);

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