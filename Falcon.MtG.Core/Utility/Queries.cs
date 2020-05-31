namespace Falcon.MtG.Utility
{
    using System.Linq;
    using Falcon.MtG;
    using Falcon.MtG.Models.Sql;
    using Microsoft.EntityFrameworkCore;

    public static class Queries
    {
        public static IQueryable<Legality> IncludeCardProperties(this IQueryable<Legality> legality) => legality
            .Include(l => l.Card)
                .ThenInclude(c => c.Colors)
            .Include(l => l.Card)
                .ThenInclude(c => c.ColorIdentity)
                    .ThenInclude(c => c.Color)
            .Include(l => l.Card)
                .ThenInclude(c => c.Printings)
                    .ThenInclude(p => p.Artist)
            .Include(l => l.Card)
                .ThenInclude(c => c.Printings)
                    .ThenInclude(p => p.Watermark)
            .Include(l => l.Card)
                .ThenInclude(c => c.Printings)
                    .ThenInclude(p => p.Set)
            .Include(l => l.Card)
                .ThenInclude(c => c.Types)
                    .ThenInclude(ct => ct.CardType)
            .Include(l => l.Card)
                .ThenInclude(c => c.Subtypes)
                    .ThenInclude(st => st.Subtype)
            .Include(l => l.Card)
                .ThenInclude(c => c.Supertypes)
                    .ThenInclude(st => st.Supertype)
            .Include(l => l.Card)
                .ThenInclude(c => c.Layout)
            .Include(l => l.Card)
                .ThenInclude(c => c.MainSide)
            .Include(l => l.Card)
                .ThenInclude(c => c.OtherSides);

        public static IQueryable<Card> IncludeCardProperties(this IQueryable<Card> card) => card
            .Include(c => c.Colors)
            .Include(c => c.ColorIdentity)
                .ThenInclude(c => c.Color)
            .Include(c => c.Printings)
                .ThenInclude(p => p.Artist)
            .Include(c => c.Printings)
                .ThenInclude(p => p.Watermark)
            .Include(c => c.Printings)
                .ThenInclude(p => p.Set)
            .Include(c => c.Types)
                    .ThenInclude(ct => ct.CardType)
            .Include(c => c.Subtypes)
                    .ThenInclude(st => st.Subtype)
            .Include(c => c.Supertypes)
                    .ThenInclude(st => st.Supertype)
            .Include(c => c.Layout)
            .Include(c => c.MainSide)
            .Include(c => c.OtherSides);

        public static IQueryable<Card> GetLegalCards(this MtGDBContext context, string format, bool allowSilver = false) => context.Legalities
            .Where(l => l.Format == format.Replace("Penny Dreadful", "Penny").Replace(" ", string.Empty)
                    && (l.Legal || (allowSilver && l.Card.Printings.All(p => p.Border.Name == "silver")))
                    && !l.Card.Supertypes.Any(t => t.Supertype.Name == "Basic")
                    && !(l.Card.Layout.Name == "meld" && l.Card.Side == "c"))
            .IncludeCardProperties()
            .Select(l => l.Card);

        public static IQueryable<Card> BasicLandFilter(this IQueryable<Card> cards)
        {
            return cards.Where(c => c.Types.Any(t => t.CardType.Name == "land")
                                 && c.Supertypes.Any(st => st.Supertype.Name == "Basic"));
        }

        public static IQueryable<Card> NonbasicLandFilter(this IQueryable<Card> cards)
        {
            return cards.Where(c => c.Types.Any(t => t.CardType.Name == "land")
                                 && !c.Supertypes.Any(st => st.Supertype.Name == "Basic"));
        }

        public static IQueryable<Card> CreatureFilter(this IQueryable<Card> cards)
        {
            return cards.Where(c => c.Types.Any(t => t.CardType.Name == "creature"));
        }

        public static IQueryable<Card> ArtifactFilter(this IQueryable<Card> cards)
        {
            return cards.Where(c => c.Types.Any(t => t.CardType.Name == "artifact"));
        }

        public static IQueryable<Card> EquipmentFilter(this IQueryable<Card> cards)
        {
            return cards.Where(c => c.Types.Any(t => t.CardType.Name == "artifact")
                                 && c.Subtypes.Any(st => st.Subtype.Name == "Equipment"));
        }

        public static IQueryable<Card> VehicleFilter(this IQueryable<Card> cards)
        {
            return cards.Where(c => c.Types.Any(t => t.CardType.Name == "artifact")
                                 && c.Subtypes.Any(st => st.Subtype.Name == "Vehicle"));
        }

        public static IQueryable<Card> EnchantmentFilter(this IQueryable<Card> cards)
        {
            return cards.Where(c => c.Types.Any(t => t.CardType.Name == "enchantment"));
        }

        public static IQueryable<Card> AuraFilter(this IQueryable<Card> cards)
        {
            return cards.Where(c => c.Types.Any(t => t.CardType.Name == "enchantment")
                                 && (c.Subtypes.Any(st => st.Subtype.Name == "aura")
                                     || c.OracleText.Contains("Bestow {")));
        }

        public static IQueryable<Card> PlaneswalkerFilter(this IQueryable<Card> cards)
        {
            return cards.Where(c => c.Types.Any(t => t.CardType.Name == "planeswalker"));
        }

        public static IQueryable<Card> SpellFilter(this IQueryable<Card> cards)
        {
            return cards.Where(c => c.Types.Any(t => t.CardType.Name == "sorcery"
                                                  || t.CardType.Name == "instant"));
        }

        public static IQueryable<Card> ManaProducingFilter(this IQueryable<Card> cards)
        {
            return cards.Where(c => !c.Supertypes.Any(st => st.Supertype.Name == "Basic")
                                 && c.OracleText.Contains("Add {"));
        }

        public static IQueryable<Card> LegendaryFilter(this IQueryable<Card> cards)
        {
            return cards.Where(c => c.Supertypes.Any(t => t.Supertype.Name == "Legendary"));
        }

        public static Card GetCardById(this MtGDBContext context, int id) => context.Cards
            .Where(c => c.ID == id)
            .IncludeCardProperties()
            .Single();
    }
}