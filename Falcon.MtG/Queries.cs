namespace Falcon.MtG
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Queries
    {
        public static IQueryable<Card> CommanderLegalCards(this MTGDBContainer db)
        {
            return from c in db.Cards
                   where c.CommanderLegal && c.IsPrimarySide
                   select c;
        }

        public static IQueryable<Card> FilterByColorIdentity(this IQueryable<Card> cards, ICollection<Color> identity)
        {
            IEnumerable<string> colorSymbols = identity.Select(ci => ci.Symbol);
            return cards.Where(c => c.ColorIdentity.Count == 0 || !c.ColorIdentity.Select(ci => ci.Symbol).Except(colorSymbols).Any());
        }

        public static IQueryable<Card> FilterByManaCost(this IQueryable<Card> cards, int min, int max)
        {
            return cards.Where(c => c.CMC >= min && c.CMC <= max);
        }

        public static IQueryable<Card> FilterBySets(this IQueryable<Card> cards, IEnumerable<string> setCodes)
        {
            return cards.Where(c => c.Sets.Select(s => s.Code).Intersect(setCodes).Any());
        }

        public static IQueryable<Card> FilterOutByOracleText(this IQueryable<Card> cards, string phrase)
        {
            return cards.Where(c => string.IsNullOrEmpty(c.OracleText) || !c.OracleText.Contains(phrase));
        }

        public static IQueryable<Card> FilterOutCard(this IQueryable<Card> cards, Card cardToRemove)
        {
            return cards.Where(c => c.ID != cardToRemove.ID);
        }

        public static IQueryable<Card> FilterOutSubtype(this IQueryable<Card> cards, string typeName)
        {
            return cards.Where(c => !c.Subtypes.Select(t => t.Name).Contains(typeName));
        }

        public static IQueryable<Card> FilterOutSupertype(this IQueryable<Card> cards, string typeName)
        {
            return cards.Where(c => !c.Supertypes.Select(t => t.Name).Contains(typeName));
        }

        public static IQueryable<Card> FilterOutType(this IQueryable<Card> cards, string typeName)
        {
            return cards.Where(c => !c.Types.Select(t => t.Name).Contains(typeName));
        }

        public static List<Card> GetBasicLands(this MTGDBContainer db, int count, ICollection<Color> colors)
        {
            Card plains = db.Cards.Where(c => c.Name == "Plains").First();
            Card island = db.Cards.Where(c => c.Name == "Island").First();
            Card swamp = db.Cards.Where(c => c.Name == "Swamp").First();
            Card mountain = db.Cards.Where(c => c.Name == "Mountain").First();
            Card forest = db.Cards.Where(c => c.Name == "Forest").First();
            Card wastes = db.Cards.Where(c => c.Name == "Wastes").First();

            var cards = new List<Card>();
            while (cards.Count < count)
            {
                if (colors.Count == 0)
                {
                    cards.Add(wastes);
                }

                foreach (var color in colors)
                {
                    if (color.Symbol == "W")
                    {
                        cards.Add(plains);
                    }
                    else if (color.Symbol == "U")
                    {
                        cards.Add(island);
                    }
                    else if (color.Symbol == "B")
                    {
                        cards.Add(swamp);
                    }
                    else if (color.Symbol == "R")
                    {
                        cards.Add(mountain);
                    }
                    else if (color.Symbol == "G")
                    {
                        cards.Add(forest);
                    }
                }
            }

            return cards;
        }

        public static IQueryable<Card> RestrictByOracleText(this IQueryable<Card> cards, string phrase)
        {
            return cards.Where(c => !string.IsNullOrEmpty(c.OracleText) && c.OracleText.Contains(phrase));
        }

        public static IQueryable<Card> RestrictCmcRange(this IQueryable<Card> cards, int min, int max)
        {
            return cards.Where(c => c.CMC >= min && c.CMC <= max);
        }

        public static IQueryable<Card> RestrictToRarity(this IQueryable<Card> cards, string rarity)
        {
            return cards.Where(c => c.Rarities.Select(r => r.Name).Contains(rarity));
        }

        public static IQueryable<Card> RestrictToSupertype(this IQueryable<Card> cards, string typeName)
        {
            return cards.Where(c => c.Supertypes.Select(t => t.Name).Contains(typeName));
        }

        public static IQueryable<Card> RestrictToType(this IQueryable<Card> cards, string typeName)
        {
            return cards.Where(c => c.Types.Select(t => t.Name).Contains(typeName));
        }

        public static IOrderedQueryable<Card> Shuffle(this IQueryable<Card> cards)
        {
            return cards.OrderBy(c => Guid.NewGuid());
        }
    }
}