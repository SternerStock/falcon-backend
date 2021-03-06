﻿namespace Falcon.MtG.Utility
{
    using System.Linq;
    using System.Threading.Tasks;
    using Falcon.MtG.Models.Json;
    using Falcon.MtG.Models.Sql;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public static class Utility
    {
        /// <summary>
        /// Matches any mana symbol on a card, including mono, split, and phyrexian.
        /// </summary>
        public const string AnyManaSymbolRegex = @"\{([WUBRGC0-9])([\/])?([WUBRGP])?\}";

        /// <summary>
        /// Matches any symbol on a card, including Mana Symbols, Tap Symbols, etc.
        /// </summary>
        public const string AnySymbolRegex = @"\{([WUBRGPTQCXS0-9])([\/])?([WUBRGP0-9])?\}";

        /// <summary>
        /// Matches any colored mana symbol on a card, including mono, split, and phyrexian. Colors
        /// will be in $1 and $3 replacement groups. $1 may include a P or 2 in the case of
        /// phyrexian mana or twobrid symbols.
        /// </summary>
        public const string ColoredManaSymbolRegex = @"\{([WUBRG2])([\/])?([WUBRGP])?\}";

        public static T ParseMtGJson<T>(string json) where T : new()
        {
            var meta = JsonConvert.DeserializeObject<JsonMeta<T>>(json);
            if (meta != null)
            {
                return meta.Data;
            }

            return default;
        }

        private static void UpsertColor(MtGDBContext db, string symbol, string name, string landName)
        {
            var existingColor = db.Colors.Where(t => t.Symbol == symbol).SingleOrDefault();
            if (existingColor == default)
            {
                existingColor = new Color()
                {
                    Symbol = symbol
                };

                db.Colors.Local.Add(existingColor);
            }

            existingColor.Name = name;
            existingColor.BasicLandName = landName;
        }

        public static void SeedColorData(this MtGDBContext db)
        {
            UpsertColor(db, "W", "White", "Plains");
            UpsertColor(db, "U", "Blue", "Island");
            UpsertColor(db, "B", "Black", "Swamp");
            UpsertColor(db, "R", "Red", "Mountain");
            UpsertColor(db, "G", "Green", "Forest");
            UpsertColor(db, "C", "Colorless", "Wastes");
        }

        public static async Task LoadLookups(this MtGDBContext db)
        {
            //await db.CardTypes.LoadAsync();
            //await db.Subtypes.LoadAsync();
            //await db.Supertypes.LoadAsync();

            //await db.Colors.LoadAsync();
            await db.Keywords.LoadAsync();
            await db.Layouts.LoadAsync();

            await db.Borders.LoadAsync();
            await db.Artists.LoadAsync();
            await db.Frames.LoadAsync();
            //await db.Pricings.LoadAsync();
            await db.Rarities.LoadAsync();
            await db.Watermarks.LoadAsync();

            await db.Blocks.LoadAsync();
            await db.SetTypes.LoadAsync();
        }
    }
}