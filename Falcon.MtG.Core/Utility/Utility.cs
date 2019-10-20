namespace Falcon.MtG
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public enum Format
    {
        Commander,
        Brawl,
        Duel,
        Frontier,
        Future,
        Legacy,
        Modern,
        Pauper,
        Penny,
        Standard,
        Vintage,
        Oathbreaker,
        TinyLeaders
    };

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
        /// will be in $1 and $3 replacement groups. $1 may include a P or 2 in the case of phyrexian
        /// mana or twobrid symbols.
        /// </summary>
        public const string ColoredManaSymbolRegex = @"\{([WUBRG2])([\/])?([WUBRGP])?\}";

        private static void UpsertColor(MtGDBContext db, string symbol, string name, string landName)
        {
            var existingColor = db.Colors.Local.Where(t => t.Symbol == symbol).SingleOrDefault();
            if (existingColor == null)
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
            await db.CardTypes.LoadAsync();
            await db.Subtypes.LoadAsync();
            await db.Supertypes.LoadAsync();
            await db.CardTypeSubtypes.LoadAsync();
            await db.CardTypeSupertypes.LoadAsync();

            await db.Colors.LoadAsync();
            await db.Keywords.LoadAsync();
            await db.Layouts.LoadAsync();

            await db.Borders.LoadAsync();
            await db.Artists.LoadAsync();
            await db.Frames.LoadAsync();
            await db.Pricings.LoadAsync();
            await db.Rarities.LoadAsync();
            await db.Watermarks.LoadAsync();

            await db.Blocks.LoadAsync();
            await db.SetTypes.LoadAsync();
        }
    }
}