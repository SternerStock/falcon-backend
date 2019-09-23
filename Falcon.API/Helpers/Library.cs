namespace Falcon.API.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Falcon.API.Models;
    using Falcon.MtG;

    public class Library
    {
        private IQueryable<Card> legalCards;
        private IQueryable<Card> legalLands;

        public Library(MTGDBContainer db, Format format)
        {
            this.LegalCards = db.LegalCards(format).FilterOutType("Land");
            this.LegalLands = db.LegalCards(format).RestrictToType("Land").FilterOutSupertype("Basic");
        }

        public IQueryable<Card> LandCards
        {
            get
            {
                return this.LegalLands;
            }
        }

        public IQueryable<Card> NonlandCards
        {
            get
            {
                return this.LegalCards;
            }
        }

        private IQueryable<Card> LegalCards { get => this.legalCards; set => this.legalCards = value; }

        private IQueryable<Card> LegalLands { get => this.legalLands; set => this.legalLands = value; }

        public void FilterArtifacts(Category artifacts, Category equipment)
        {
            if (!artifacts.Enabled)
            {
                this.LegalCards = this.LegalCards.FilterOutType("Artifact");
                this.LegalLands = this.LegalLands.FilterOutType("Artifact");
            }
            else if (!equipment.Enabled)
            {
                this.LegalCards = this.LegalCards.FilterOutSubtype("Equipment").FilterOutSubtype("Fortification");
            }
            else if (artifacts.Count == equipment.Count)
            {
                // All artifacts should be equipment, so filter out non-equipment artifacts
                this.LegalCards = this.LegalCards.Where(c => !c.Types.Select(t => t.Name).Contains("Artifact") || c.Subtypes.Select(t => t.Name).Contains("Equipment") || c.Subtypes.Select(t => t.Name).Contains("Fortification"));
                this.LegalLands = this.LegalLands.FilterOutType("Artifact");
            }
        }

        public void FilterByCmc(Range cmc)
        {
            this.LegalCards = this.LegalCards.RestrictCmcRange(cmc.Min, cmc.Max);
        }

        public void FilterCreatures(Category creatures)
        {
            if (!creatures.Enabled)
            {
                this.LegalCards = this.LegalCards.FilterOutType("Creature");
                this.LegalLands = this.LegalLands.FilterOutType("Creature");
            }
        }

        public void FilterEnchantments(Category enchantments, Category auras)
        {
            if (!enchantments.Enabled)
            {
                this.LegalCards = this.LegalCards.FilterOutType("Enchantment");
            }
            else if (!auras.Enabled)
            {
                this.LegalCards = this.LegalCards.FilterOutSubtype("Aura").FilterOutByOracleRegex("Bestow");
            }
            else if (enchantments.Count == auras.Count)
            {
                // All enchantments should be auras, so filter out non-aura enchantments
                this.LegalCards = this.LegalCards.Where(c => !c.Types.Select(t => t.Name).Contains("Enchantment") || c.Subtypes.Select(t => t.Name).Contains("Aura") ||
                   (!string.IsNullOrEmpty(c.OracleText) && c.OracleText.Contains("Bestow")));
            }
        }

        public void FilterLegendary(Category legendary, bool maxed)
        {
            if (!legendary.Enabled)
            {
                this.LegalCards = this.LegalCards.FilterOutSupertype("Legendary");
                this.LegalLands = this.LegalLands.FilterOutSupertype("Legendary");
            }
            else if (maxed)
            {
                this.LegalCards = this.LegalCards.RestrictToSupertype("Legendary");
                this.LegalLands = this.LegalLands.RestrictToSupertype("Legendary");
            }
        }

        public void FilterManaProducers(Category manaProducing, bool maxed)
        {
            if (!manaProducing.Enabled)
            {
                this.LegalCards = this.LegalCards.FilterOutByOracleRegex("Add " + Utility.AnyManaSymbolRegex);
                this.LegalLands = this.LegalLands.FilterOutByOracleRegex("Add " + Utility.AnyManaSymbolRegex);
            }
            else if (maxed)
            {
                this.LegalCards = this.LegalCards.RestrictByOracleRegex("Add " + Utility.AnyManaSymbolRegex);
                this.LegalLands = this.LegalLands.RestrictByOracleRegex("Add " + Utility.AnyManaSymbolRegex);
            }
        }

        public void FilterNonbasicLands(Category nonbasicLands)
        {
            if (!nonbasicLands.Enabled)
            {
                this.LegalCards.FilterOutType("Land");
            }
        }

        public void FilterPlaneswalkers(Category planeswalkers)
        {
            if (!planeswalkers.Enabled)
            {
                this.LegalCards = this.LegalCards.FilterOutType("Planeswalker");
            }
        }

        public void FilterSets(IEnumerable<string> setCodes)
        {
            if (setCodes.Count() > 0)
            {
                this.LegalCards = this.LegalCards.FilterBySets(setCodes);
                this.LegalLands = this.LegalLands.FilterBySets(setCodes);
            }
        }

        public void FilterSpells(Category spells)
        {
            if (!spells.Enabled)
            {
                this.LegalCards = this.LegalCards.FilterOutType("Instant").FilterOutType("Sorcery");
            }
        }
    }
}