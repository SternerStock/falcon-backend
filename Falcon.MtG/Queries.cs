namespace Falcon.MtG
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class Queries
    {
        public static IQueryable<Card> LegalCards(this MTGDBContainer db, Format format = Format.Commander, bool cmdrsOnly = false)
        {
            var legalities = db.Legalities.Where(l => l.Format == format.ToString());
            
            if (cmdrsOnly)
            {
                legalities = legalities.Where(l => l.LegalAsCommander);
            }
            else
            {
                legalities = legalities.Where(l => l.Legal);
            }

            var test = legalities.ToList();

            var cards = db.Cards.Where(c => c.Legalities.Intersect(legalities).Any());

            if (format == Format.Pauper)
            {
                if (cmdrsOnly)
                {
                    cards = cards.Where(c => c.Printings.Select(p => p.Rarity).Select(r => r.Name).Contains("Uncommon"));
                }
                else
                {
                    cards = cards.Where(c => c.Printings.Select(p => p.Rarity).Select(r => r.Name).Contains("Common"));
                }
            }

            return cards;
        }

        public static IQueryable<Card> GetCommanders(this MTGDBContainer db, Format format = Format.Commander)
        {
            var cmdrTypes = new List<string>() { "Creature" };

            if (format == Format.Brawl || format == Format.TinyLeaders)
            {
                cmdrTypes.Add("Planeswalker");
            }

            var cards = db.LegalCards(format, true);
            
            if (format == Format.Pauper)
            {
                cards = cards.Where(c => cmdrTypes.Any(s => c.Types.Select(t => t.Name).Contains(s)) || c.OracleText.Contains("can be your commander."));
            }
            else
            {
                cards = cards.Where(c => (cmdrTypes.Any(s => c.Types.Select(t => t.Name).Contains(s)) && c.Supertypes.Select(t => t.Name).Contains("Legendary")) || c.OracleText.Contains("can be your commander."));
            }

            return cards;
        }

        public static bool HasKeyword(this Card card, string keyword)
        {
            return !string.IsNullOrEmpty(card.OracleText) &&
                (card.OracleText.Contains("\n" + keyword) ||
                    card.OracleText.StartsWith(keyword) ||
                    card.OracleText.Contains(", " + keyword));
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
            return cards.Where(c => c.Printings.Select(p => p.Set).Select(s => s.Code).Intersect(setCodes).Any());
        }

        public static IQueryable<Card> FilterOutByOracleRegex(this IQueryable<Card> cards, string pattern)
        {
            var regex = new Regex(pattern);
            return cards.Where(c => string.IsNullOrEmpty(c.OracleText) || !regex.IsMatch(c.OracleText));
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

        public static List<Card> GetBasicLands(this MTGDBContainer db, int count, ICollection<Color> colors, Format format)
        {
            Card plains = db.Cards.Where(c => c.Name == "Plains").First();
            Card island = db.Cards.Where(c => c.Name == "Island").First();
            Card swamp = db.Cards.Where(c => c.Name == "Swamp").First();
            Card mountain = db.Cards.Where(c => c.Name == "Mountain").First();
            Card forest = db.Cards.Where(c => c.Name == "Forest").First();
            
            Card colorlessFiller = db.LegalCards(format).Where(c => c.Name == "Wastes").FirstOrDefault();
            if (colorlessFiller == default(Card))
            {
                var rng = new Random();
                var basicLands = new Card[] { plains, island, swamp, mountain, forest };

                colorlessFiller = basicLands[rng.Next(0, basicLands.Length)];
            }

            var cards = new List<Card>();
            while (cards.Count < count)
            {
                if (colors.Count == 0)
                {
                    cards.Add(colorlessFiller);
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

        public static IQueryable<Card> RestrictByOracleRegex(this IQueryable<Card> cards, string pattern)
        {
            var regex = new Regex(pattern);
            return cards.Where(c => !string.IsNullOrEmpty(c.OracleText) || regex.IsMatch(c.OracleText));
        }

        public static IQueryable<Card> RestrictCmcRange(this IQueryable<Card> cards, int min, int max)
        {
            return cards.Where(c => c.CMC >= min && c.CMC <= max);
        }

        public static IQueryable<Card> RestrictToRarity(this IQueryable<Card> cards, string rarity)
        {
            return cards.Where(c => c.Printings.Select(p => p.Rarity).Select(r => r.Name).Contains(rarity));
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