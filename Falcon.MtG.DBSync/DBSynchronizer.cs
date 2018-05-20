namespace Falcon.MtG.DBSync
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    public class DBSynchronizer : IDisposable
    {
        private MTGDBContainer db;
        private bool disposedValue = false;
        private string workingDirectory;
        private LegalityHelper legalityHelper;
        private List<string> abilityKeywords;
        private const string KeywordListFileName = "KeywordAbilities.txt";
        private readonly string[] WhoWhatWhereWhenWhy = { "Who", "What", "Where", "When", "Why" };

        public DBSynchronizer(string workingDir)
        {
            this.workingDirectory = workingDir;
            this.db = new MTGDBContainer();
            this.db.Configuration.AutoDetectChangesEnabled = false;
            this.db.Sets.Load();
            this.db.Abilities.Load();
            this.db.Colors.Load();
            this.db.Supertypes.Load();
            this.db.Types.Load();
            this.db.Subtypes.Load();
            this.db.Cards.Load();
            this.db.Rarities.Load();
            legalityHelper = new LegalityHelper(workingDir);

            var keywordsFileContent = File.ReadAllText(Path.Combine(workingDirectory, KeywordListFileName));
            var keywords = keywordsFileContent.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            abilityKeywords = keywords.Select(b => b.Trim()).ToList();
        }

        public DBSynchronizer() : this(Environment.CurrentDirectory)
        {
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing).
            this.Dispose(true);
        }

        public void Sync(IEnumerable<string> setsToUpdate)
        {
            foreach (var setFileName in setsToUpdate)
            {
                string setJson = File.ReadAllText(Path.Combine(this.workingDirectory, setFileName));
                if (string.IsNullOrEmpty(setJson))
                {
                    Console.WriteLine("Skipping " + setFileName + ": file empty.");
                    continue;
                }

                JsonSet setData = JsonConvert.DeserializeObject<JsonSet>(setJson);

                this.SyncSet(setData);
            }

            this.SaveChanges();
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

        private HashSet<Color> GetMissingColors(ICollection<Color> existingColors, IEnumerable<string> potentialColors)
        {
            var colorsToAdd = new HashSet<Color>();

            foreach (var color in potentialColors)
            {
                var dbColor = this.db.Colors.Local.SingleOrDefault(c => c.Name == color);

                if (dbColor == null)
                {
                    dbColor = this.db.Colors.Create();
                    dbColor.Name = color;
                    string symbol = color[0].ToString();
                    if (color == "Blue")
                    {
                        symbol = "U";
                    }

                    dbColor.Symbol = symbol;
                    dbColor = this.db.Colors.Add(dbColor);
                }

                if (!existingColors.Contains(dbColor))
                {
                    colorsToAdd.Add(dbColor);
                }
            }

            return colorsToAdd;
        }

        private void SaveChanges()
        {
            try
            {
                this.db.Configuration.AutoDetectChangesEnabled = true;
                if (this.db.ChangeTracker.HasChanges())
                {
                    this.db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                this.db.Configuration.AutoDetectChangesEnabled = false;
            }
        }

        private void SyncCard(Set dbSet, JsonCard fileCard)
        {
            var dbCard = this.db.Cards.Local.SingleOrDefault(c => c.Name == fileCard.Name);

            if (dbCard == null)
            {
                dbCard = this.db.Cards.Create();
                dbCard.Name = fileCard.Name;
                dbCard.LatestPrintDate = dbSet.Date;
                dbCard = this.db.Cards.Add(dbCard);
            }

            if (!dbCard.Sets.Contains(dbSet))
            {
                dbCard.Sets.Add(dbSet);
            }

            DateTime latestPrinting = DateTime.MinValue;
            foreach (var set in dbCard.Sets)
            {
                if (set.Date > latestPrinting)
                {
                    latestPrinting = set.Date;
                }
            }

            if (latestPrinting == DateTime.MinValue || dbCard.LatestPrintDate <= latestPrinting)
            {
                // Only get multiverseId and flavor text for latest printing of the card.
                if (fileCard.MultiverseId.HasValue && dbCard.MultiverseId != fileCard.MultiverseId)
                {
                    dbCard.MultiverseId = fileCard.MultiverseId;
                }

                if (dbCard.LatestPrintDate != latestPrinting)
                {
                    dbCard.LatestPrintDate = latestPrinting;
                }

                if (!string.IsNullOrEmpty(fileCard.Flavor) && dbCard.FlavorText != fileCard.Flavor)
                {
                    dbCard.FlavorText = fileCard.Flavor;
                }
            }

            if (dbCard.ManaCost != fileCard.ManaCost)
            {
                dbCard.ManaCost = fileCard.ManaCost;
            }

            int parsedCmc = Convert.ToInt32(fileCard.CMC);
            if (dbCard.CMC != parsedCmc)
            {
                dbCard.CMC = parsedCmc;
            }

            if (dbCard.TypeLine != fileCard.Type)
            {
                dbCard.TypeLine = fileCard.Type;
            }

            if (dbCard.OracleText != fileCard.OracleText)
            {
                dbCard.OracleText = fileCard.OracleText;
            }

            HashSet<Color> colorsToAdd = this.GetMissingColors(dbCard.Colors, fileCard.Colors);
            foreach (var dbColor in colorsToAdd)
            {
                dbCard.Colors.Add(dbColor);
            }

            var colorIdentity = new HashSet<string>();
            foreach (var color in fileCard.Colors)
            {
                colorIdentity.Add(color);
            }

            HashSet<string> parsedColors = Utility.GetColorIdentityFromText(fileCard.ManaCost);
            foreach (var color in parsedColors)
            {
                colorIdentity.Add(color);
            }

            parsedColors = Utility.GetColorIdentityFromText(fileCard.OriginalText);
            foreach (var color in parsedColors)
            {
                colorIdentity.Add(color);
            }

            parsedColors = Utility.GetColorIdentityFromTypes(fileCard.Subtypes);
            foreach (var color in parsedColors)
            {
                colorIdentity.Add(color);
            }

            colorsToAdd = this.GetMissingColors(dbCard.ColorIdentity, colorIdentity);
            foreach (var dbColor in colorsToAdd)
            {
                dbCard.ColorIdentity.Add(dbColor);
            }

            int parsedPower = 0;
            int.TryParse(fileCard.Power, out parsedPower);

            if (dbCard.Power != parsedPower)
            {
                dbCard.Power = parsedPower;
            }

            int parsedToughness = 0;
            int.TryParse(fileCard.Toughness, out parsedToughness);

            if (dbCard.Toughness != parsedToughness)
            {
                dbCard.Toughness = parsedToughness;
            }

            this.SyncLegalities(dbCard, fileCard);

            foreach(var keyword in abilityKeywords)
            {
                this.SyncKeywordAbility(dbCard, keyword);
            }

            if (dbCard.IsPrimarySide != fileCard.IsPrimarySide)
            {
                dbCard.IsPrimarySide = fileCard.IsPrimarySide;
            }

            this.SyncRarity(dbCard, fileCard.Rarity);

            foreach (var supertype in fileCard.Supertypes)
            {
                this.SyncSupertype(dbCard, supertype);
            }

            foreach (var type in fileCard.Types)
            {
                this.SyncType(dbCard, type);
            }

            foreach (var subtype in fileCard.Subtypes)
            {
                this.SyncSubtype(dbCard, subtype);
            }
        }

        private void SyncRarity(Card dbCard, string rarity)
        {
            var dbRarity = this.db.Rarities.Local.SingleOrDefault(r => r.Name == rarity);

            if (dbRarity == null)
            {
                dbRarity = this.db.Rarities.Create();
                dbRarity.Name = rarity;
                dbRarity = this.db.Rarities.Add(dbRarity);
            }

            if (!dbCard.Rarities.Contains(dbRarity))
            {
                dbCard.Rarities.Add(dbRarity);
            }
        }

        private void SyncLegalities(Card dbCard, JsonCard fileCard)
        {
            var formats = Enum.GetValues(typeof(EdhFormat));

            foreach(var format in formats)
            {
                var fileLegality = legalityHelper.GetLegality((EdhFormat)format, fileCard);

                var dbLegality = this.db.Legalities.Local.SingleOrDefault(l =>
                    l.Format == fileLegality.Format &&
                    l.Legal == fileLegality.Legal &&
                    l.LegalAsCommander == fileLegality.LegalAsCommander);

                if (dbLegality == null)
                {
                    dbLegality = this.db.Legalities.Create();
                    dbLegality.Format = fileLegality.Format;
                    dbLegality.Legal = fileLegality.Legal;
                    dbLegality.LegalAsCommander = fileLegality.LegalAsCommander;
                    dbLegality = this.db.Legalities.Add(dbLegality);
                }

                if (!dbCard.Legalities.Contains(dbLegality))
                {
                    dbCard.Legalities.Add(dbLegality);
                }
            }
        }

        private void SyncKeywordAbility(Card dbCard, string keyword)
        {
            var dbAbility = this.db.Abilities.Local.SingleOrDefault(a => a.Name == keyword);

            if (dbAbility == null)
            {
                dbAbility = this.db.Abilities.Create();
                dbAbility.Name = keyword;
                dbAbility.Type = "Keyword";
                dbAbility = this.db.Abilities.Add(dbAbility);
            }

            if (dbCard.HasKeyword(keyword) && !dbCard.Abilities.Contains(dbAbility))
            {
                dbCard.Abilities.Add(dbAbility);
            }
        }

        private void SyncSet(JsonSet fileSet)
        {
            //if (fileSet.Border == "silver")
            //{
            //    // Don't deal with Silver-bordered stuff. Too wacky to parse.
            //    return;
            //}

            Console.WriteLine("Syncing set: " + fileSet.Name);
            var twoFaceCards = new List<JsonCard>();
            int cardCount = 0;

            DateTime releaseDate = DateTime.MinValue;
            DateTime.TryParse(fileSet.ReleaseDate, out releaseDate);

            var dbSet = this.db.Sets.Local.SingleOrDefault(s => s.Code == fileSet.Code);
            if (dbSet == null)
            {
                dbSet = this.db.Sets.Create();
                dbSet = this.db.Sets.Add(dbSet);
            }

            if (dbSet.Code != fileSet.Code)
            {
                dbSet.Code = fileSet.Code;
            }

            if (dbSet.Name != fileSet.Name)
            {
                dbSet.Name = fileSet.Name;
            }

            if (dbSet.Block != fileSet.Block)
            {
                dbSet.Block = fileSet.Block;
            }

            if (dbSet.Date != releaseDate)
            {
                dbSet.Date = releaseDate;
            }

            if (dbSet.Border != fileSet.Border)
            {
                dbSet.Border = fileSet.Border;
            }

            if (dbSet.Type != fileSet.Type)
            {
                dbSet.Type = fileSet.Type;
            }

            foreach (var fileCard in fileSet.Cards)
            {
                this.SyncCard(dbSet, fileCard);

                if (fileCard.Names.Count > 1)
                {
                    twoFaceCards.Add(fileCard);
                }

                cardCount++;
                Console.Write(cardCount + "/" + fileSet.Cards.Count + "\r");
            }

            this.SaveChanges();

            foreach (var twoFaceCard in twoFaceCards)
            {

                if (WhoWhatWhereWhenWhy.Contains(twoFaceCard.Name))
                {
                    var dbCard = this.db.Cards.SingleOrDefault(c => c.Name == twoFaceCard.Name);

                    if (dbCard != null)
                    {
                        foreach (var color in db.Colors)
                        {
                            if (!dbCard.ColorIdentity.Contains(color))
                            {
                                dbCard.ColorIdentity.Add(color);
                            }
                        }
                    }
                }
                else
                {
                    var firstSide = this.db.Cards.SingleOrDefault(c => c.Name == twoFaceCard.Name);

                    foreach (var altName in twoFaceCard.Names)
                    {
                        if (altName != twoFaceCard.Name)
                        {
                            var otherSide = this.db.Cards.SingleOrDefault(c => c.Name == altName);
                            if (otherSide != null)
                            {
                                if (twoFaceCard.Layout != "meld" && firstSide.OtherSide != otherSide)
                                {
                                    firstSide.OtherSide = otherSide;
                                }

                                foreach (var color in otherSide.ColorIdentity)
                                {
                                    if (!firstSide.ColorIdentity.Contains(color))
                                    {
                                        firstSide.ColorIdentity.Add(color);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            this.SaveChanges();
        }

        private void SyncSubtype(Card dbCard, string subtype)
        {
            var dbSubtype = this.db.Subtypes.Local.SingleOrDefault(t => t.Name == subtype);

            if (dbSubtype == null)
            {
                dbSubtype = this.db.Subtypes.Create();
                dbSubtype.Name = subtype;
                dbSubtype = this.db.Subtypes.Add(dbSubtype);
            }

            if (!dbCard.Subtypes.Contains(dbSubtype))
            {
                dbCard.Subtypes.Add(dbSubtype);
            }
        }

        private void SyncSupertype(Card dbCard, string supertype)
        {
            var dbSuperType = this.db.Supertypes.Local.SingleOrDefault(t => t.Name == supertype);

            if (dbSuperType == null)
            {
                dbSuperType = this.db.Supertypes.Create();
                dbSuperType.Name = supertype;
                dbSuperType = this.db.Supertypes.Add(dbSuperType);
            }

            if (!dbCard.Supertypes.Contains(dbSuperType))
            {
                dbCard.Supertypes.Add(dbSuperType);
            }
        }

        private void SyncType(Card dbCard, string type)
        {
            var dbType = this.db.Types.Local.SingleOrDefault(t => t.Name == type);

            if (dbType == null)
            {
                dbType = this.db.Types.Create();
                dbType.Name = type;
                dbType = this.db.Types.Add(dbType);
            }

            if (!dbCard.Types.Contains(dbType))
            {
                dbCard.Types.Add(dbType);
            }
        }
    }
}