namespace Falcon.MtG
{
    using System.Data.Entity;
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

        private static async Task UpsertColor(MTGDBContainer db, string symbol, string name, string landName)
        {
            var existingColor = db.Colors.Local.Where(t => t.Symbol == symbol).SingleOrDefault();
            if (existingColor == null)
            {
                existingColor = await db.Colors.Where(t => t.Symbol == symbol).SingleOrDefaultAsync();
            }

            if (existingColor == null)
            {
                existingColor = db.Colors.Add(new Color()
                {
                    Symbol = symbol
                });
            }

            existingColor.Name = name;
            existingColor.BasicLandName = landName;
        }

        public static async Task SeedColorData(this MTGDBContainer db)
        {
            await UpsertColor(db, "W", "White", "Plains");
            await UpsertColor(db, "U", "Blue", "Island");
            await UpsertColor(db, "B", "Black", "Swamp");
            await UpsertColor(db, "R", "Red", "Mountain");
            await UpsertColor(db, "G", "Green", "Forest");
            await UpsertColor(db, "C", "Colorless", "Wastes");
        }

        public static async Task LoadLocalDB(this MTGDBContainer db)
        {
            await db.Subtypes
                .Include(t => t.Types)
                .LoadAsync();

            await db.Supertypes
                .Include(t => t.Types)
                .LoadAsync();

            await db.CardTypes
                .Include(t => t.Subtypes)
                .Include(t => t.Supertypes)
                .LoadAsync();

            await db.Colors.LoadAsync();
            await db.Keywords.LoadAsync();
            await db.Layouts.LoadAsync();
            await db.Legalities.LoadAsync();

            await db.Cards
                .Include(c => c.Colors)
                .Include(c => c.ColorIdentity)
                .Include(c => c.Supertypes)
                .Include(c => c.Types)
                .Include(c => c.Subtypes)
                .Include(c => c.Layout)
                .Include(c => c.Legalities)
                .Include(c => c.Keywords)
                .Include(c => c.MainSide)
                .LoadAsync();

            await db.Borders.LoadAsync();
            await db.Artists.LoadAsync();
            await db.Frames.LoadAsync();
            await db.Pricings.LoadAsync();
            await db.Rarities.LoadAsync();
            await db.Watermarks.LoadAsync();

            await db.Printings
                .Include(p => p.Card)
                .Include(p => p.Set)
                .Include(p => p.Artist)
                .Include(p => p.Watermark)
                .Include(p => p.Frame)
                .Include(p => p.Rarity)
                .Include(p => p.Border)
                .Include(p => p.Pricings)
                .LoadAsync();

            await db.Blocks.LoadAsync();
            await db.SetTypes.LoadAsync();
            await db.Sets
                .Include(s => s.Block)
                .Include(s => s.SetType)
                .Include(s => s.Printings)
                .LoadAsync();
        }
    }
}