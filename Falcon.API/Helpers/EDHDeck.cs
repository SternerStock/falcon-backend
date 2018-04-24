namespace Falcon.API.Helpers
{
    using System.Collections.Generic;
    using Falcon.MtG;
    using Falcon.API.Models;

    public class EDHDeck
    {
        public const int MaxDeckSize = 100;
        private int artifactsNeeded;
        private int aurasNeeded;
        private int basicLandsNeeded;
        private int creaturesNeeded;
        private int enchantmentsNeeded;
        private int equipmentNeeded;
        private int legendaryNeeded;
        private int manaProducingNeeded;
        private int nonbasicLandsNeeded;
        private int planeswalkersNeeded;
        private int sharesTypesNeeded;
        private int spellsNeeded;

        public EDHDeck()
        {
            this.Cards = new List<Card>();
        }

        public EDHDeck(Card cmdr, GeneratorRequestModel options) : this()
        {
            this.Commander = cmdr;
            this.Cards.Add(this.Commander);

            this.basicLandsNeeded = options.BasicLands.Count;
            this.nonbasicLandsNeeded = options.NonbasicLands.Count;
            this.creaturesNeeded = options.Creatures.Count;
            this.artifactsNeeded = options.Artifacts.Count;
            this.equipmentNeeded = options.Equipment.Count;
            this.enchantmentsNeeded = options.Enchantments.Count;
            this.aurasNeeded = options.Auras.Count;
            this.planeswalkersNeeded = options.Planeswalkers.Count;
            this.spellsNeeded = options.Spells.Count;
            this.manaProducingNeeded = options.ManaProducing.Count;
            this.sharesTypesNeeded = options.SharesTypes.Count;
            this.legendaryNeeded = options.Legendary.Count;
        }

        public int BasicLandsNeeded
        {
            get
            {
                return this.basicLandsNeeded;
            }
        }

        public List<Card> Cards { get; set; }

        public Card Commander { get; set; }

        public int FillerNeeded
        {
            get
            {
                return this.Cards.Count - this.basicLandsNeeded;
            }
        }

        public bool IsFull
        {
            get
            {
                return this.Cards.Count == MaxDeckSize;
            }
        }

        public bool IsFullExceptForBasicLands
        {
            get
            {
                return this.Cards.Count >= MaxDeckSize - this.basicLandsNeeded;
            }
        }

        public bool LandMinimumMet
        {
            get
            {
                return this.nonbasicLandsNeeded <= 0;
            }
        }

        public bool NonlandMinimumsMet
        {
            get
            {
                return this.creaturesNeeded <= 0 &&
                       this.artifactsNeeded <= 0 &&
                       this.equipmentNeeded <= 0 &&
                       this.enchantmentsNeeded <= 0 &&
                       this.aurasNeeded <= 0 &&
                       this.spellsNeeded <= 0 &&
                       this.planeswalkersNeeded <= 0 &&
                       this.manaProducingNeeded <= 0 &&
                       this.sharesTypesNeeded <= 0 &&
                       this.legendaryNeeded <= 0;
            }
        }

        public bool AddCard(Card candidate, bool secondPass = false)
        {
            bool addCard = secondPass || this.NonlandMinimumsMet;

            if (candidate.IsCreature && this.creaturesNeeded > 0)
            {
                this.creaturesNeeded--;
                addCard = true;
            }

            if (candidate.IsArtifact && this.artifactsNeeded > 0)
            {
                this.artifactsNeeded--;
                addCard = true;

                if (candidate.IsEquipment && this.equipmentNeeded > 0)
                {
                    this.equipmentNeeded--;
                }
            }

            if (candidate.IsEnchantment && this.enchantmentsNeeded > 0)
            {
                this.enchantmentsNeeded--;
                addCard = true;

                if (candidate.IsAura && this.aurasNeeded > 0)
                {
                    this.aurasNeeded--;
                }
            }

            if (candidate.IsPlaneswalker && this.planeswalkersNeeded > 0)
            {
                this.planeswalkersNeeded--;
                addCard = true;
            }

            if (candidate.IsSpell && this.spellsNeeded > 0)
            {
                this.spellsNeeded--;
                addCard = true;
            }

            if (candidate.ProducesMana && this.manaProducingNeeded > 0)
            {
                this.manaProducingNeeded--;
                addCard = true;
            }

            if (candidate.SharesSubtype(this.Commander.Subtypes) && this.sharesTypesNeeded > 0)
            {
                this.sharesTypesNeeded--;
                addCard = true;
            }

            if (candidate.IsLegendary && this.legendaryNeeded > 0)
            {
                this.legendaryNeeded--;
                addCard = true;
            }

            if (candidate.IsNonbasicLand && this.nonbasicLandsNeeded > 0)
            {
                this.nonbasicLandsNeeded--;
                addCard = true;
            }

            if (addCard)
            {
                this.Cards.Add(candidate);
            }

            return addCard;
        }

        public override string ToString()
        {
            return "SB: 1 " + string.Join("\r\n1 ", this.Cards);
        }
    }
}