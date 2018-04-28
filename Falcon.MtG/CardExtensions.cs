namespace Falcon.MtG
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public partial class Card
    {
        public bool IsArtifact
        {
            get
            {
                return this.HasType("Artifact");
            }
        }

        public bool IsAura
        {
            get
            {
                return this.HasSubtype("Aura") || this.ContainsPhrase("Bestow");
            }
        }

        public bool IsCreature
        {
            get
            {
                return this.HasType("Creature");
            }
        }

        public bool IsEnchantment
        {
            get
            {
                return this.HasType("Enchantment");
            }
        }

        public bool IsEquipment
        {
            get
            {
                return this.HasSubtype("Equipment") || this.HasSubtype("Fortification");
            }
        }

        public bool IsLegendary
        {
            get
            {
                return this.HasSupertype("Legendary");
            }
        }

        public bool IsNonbasicLand
        {
            get
            {
                return this.HasType("Land") && !this.HasSupertype("Basic");
            }
        }

        public bool IsPlaneswalker
        {
            get
            {
                return this.HasType("Planeswalker");
            }
        }

        public bool IsSpell
        {
            get
            {
                return this.HasType("Instant") || this.HasType("Sorcery");
            }
        }

        public bool ProducesMana
        {
            get
            {
                return this.MatchesRegex(Utility.AnyManaSymbolRegex);
            }
        }

        public bool ContainsPhrase(string phrase)
        {
            return !string.IsNullOrEmpty(this.OracleText) && this.OracleText.Contains(phrase);
        }

        public bool HasSubtype(string typeName)
        {
            return this.Subtypes.Select(t => t.Name).Contains(typeName);
        }

        public bool HasSupertype(string typeName)
        {
            return this.Supertypes.Select(t => t.Name).Contains(typeName);
        }

        public bool HasType(string typeName)
        {
            return this.Types.Select(t => t.Name).Contains(typeName);
        }

        public bool MatchesRegex(Regex regex)
        {
            return !string.IsNullOrEmpty(this.OracleText) && regex.IsMatch(this.OracleText);
        }

        public bool MatchesRegex(string regex)
        {
            return MatchesRegex(new Regex(regex));
        }

        public bool SharesSubtype(IEnumerable<Subtype> subtypes)
        {
            return this.Subtypes.Intersect(subtypes).Any();
        }

        /// <summary>
        /// Return a row for pasting a deck into Cockatrice.
        /// </summary>
        /// <returns>
        /// A name in the form of "1 Black Lotus" or "1 Breaking/Entering" if a split card.
        /// </returns>
        public override string ToString()
        {
            string[] WhoWhatWhereWhenWhy = { "Who", "What", "Where", "When", "Why" };

            if (WhoWhatWhereWhenWhy.Contains(this.Name))
            {
                return "Who/What/Where/When/Why";
            }
            
            if (this.OtherSide != null && this.MultiverseId == this.OtherSide.MultiverseId &&
                (this.Subtypes.Select(t => t.Name).Contains("Instant") ||
                 this.Subtypes.Select(t => t.Name).Contains("Sorcery")))
            {
                if (this.IsPrimarySide)
                {
                    return this.Name + "/" + this.OtherSide.Name;
                }
                else
                {
                    return this.OtherSide.Name + "/" + this.Name;
                }
            }

            return this.Name;
        }
    }
}