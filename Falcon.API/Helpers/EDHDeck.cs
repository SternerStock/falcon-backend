namespace Falcon.API.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Falcon.API.Models;
    using Falcon.MtG;

    public class EDHDeck
    {
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
        private int reqDeckSize;
        private int sharesTypesNeeded;
        private int spellsNeeded;

        public EDHDeck()
        {
            this.Cards = new List<Card>();
        }

        public EDHDeck(GeneratorRequestModel options, Card cmdr1, Card cmdr2 = null) : this()
        {
            this.Commanders = new List<Card>
            {
                cmdr1
            };

            if (cmdr2 != null)
            {
                this.Commanders.Add(cmdr2);
            }

            this.Cards.AddRange(this.Commanders);

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

            switch (options.Format)
            {
                case EdhFormat.Commander:
                case EdhFormat.Pauper:
                    this.ReqDeckSize = 100;
                    break;

                case EdhFormat.Brawl:
                    this.ReqDeckSize = 60;
                    break;

                case EdhFormat.TinyLeaders:
                    this.ReqDeckSize = 50;
                    break;
            }
        }

        public int BasicLandsNeeded
        {
            get
            {
                return this.basicLandsNeeded;
            }
        }

        public List<Card> Cards { get; set; }

        public List<Color> ColorIdentity
        {
            get
            {
                return this.Commanders.SelectMany(c => c.ColorIdentity).Distinct().ToList();
            }
        }

        public List<Card> Commanders { get; set; }

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
                return this.Cards.Count == ReqDeckSize;
            }
        }

        public bool IsFullExceptForBasicLands
        {
            get
            {
                return this.Cards.Count >= ReqDeckSize - this.basicLandsNeeded;
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

        public int ReqDeckSize { get => this.reqDeckSize; set => this.reqDeckSize = value; }

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

            if (candidate.SharesSubtype(this.Commanders.SelectMany(c => c.Subtypes)) && this.sharesTypesNeeded > 0)
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
            string decklist = "";

            foreach (var card in this.Cards.GroupBy(c => c.Name))
            {
                if (this.Commanders.Select(c => c.Name).Contains(card.Key))
                {
                    decklist += "SB: ";
                }

                decklist += card.Count() + " " + card.Key + "\r\n";
            }

            return decklist.TrimEnd("\r\n".ToCharArray());
        }
    }
}