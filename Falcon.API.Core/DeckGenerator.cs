namespace Falcon.API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Falcon.API.DTO;
    using Falcon.MtG;
    using Falcon.MtG.Models;
    using Falcon.MtG.Models.Sql;
    using Falcon.MtG.Utility;
    using Microsoft.EntityFrameworkCore;

    public class DeckGenerator
    {
        private readonly MtGDBContext context;
        private readonly Random RNG;
        private readonly Deck deck;
        private readonly GenerateDeckDto settings;

        private readonly string format;

        private readonly List<int> disabledColorIdentityIds;
        private readonly List<int> disabledCardSupertypeIds;
        private readonly List<int> disabledCardTypeIds;
        private readonly List<int> disabledCardSubtypeIds;

        private readonly List<int> commanderCreatureTypeIds;

        public DeckGenerator(MtGDBContext context, GenerateDeckDto settings)
        {
            this.context = context;
            int seed = settings.Seed ?? Guid.NewGuid().GetHashCode();
            this.RNG = new Random(seed);

            this.settings = settings;

            if (settings.DeckType == "Commander")
            {
                Card commander = this.context.GetCardById(settings.CommanderId.Value);
                Card partner = null;
                if (settings.PartnerId.HasValue)
                {
                    partner = this.context.GetCardById(settings.PartnerId.Value);
                }

                this.commanderCreatureTypeIds = new List<int>();
                if (commander?.OracleText?.Contains("is every creature type") != true && partner?.OracleText?.Contains("is every creature type") != true)
                {
                    var creatureTypes = this.context.Subtypes
                        .Include(st => st.Cards)
                        .Where(st => st.Types.Any(t => t.CardType.Name == "Creature"));

                    if (partner != null)
                    {
                        creatureTypes = creatureTypes.Where(st => st.Cards.Any(c => c.CardID == commander.ID || c.CardID == partner.ID));
                    }
                    else
                    {
                        creatureTypes = creatureTypes.Where(st => st.Cards.Any(c => c.CardID == commander.ID));
                    }

                    this.commanderCreatureTypeIds.AddRange(creatureTypes.Select(st => st.ID));
                }

                this.deck = new EdhDeck()
                {
                    Commander = commander,
                    Partner = partner,
                    Singleton = true,
                    Seed = seed
                };
            }
            else if (settings.DeckType == "Oathbreaker")
            {
                this.deck = new OathbreakerDeck()
                {
                    Oathbreaker = this.context.GetCardById(settings.CommanderId.Value),
                    SignatureSpell = this.context.GetCardById(settings.SignatureSpellId.Value),
                    Singleton = true,
                    Seed = seed
                };
            }
            else
            {
                this.deck = new Deck()
                {
                    Seed = seed
                };
            }

            this.format = settings.Format.Replace("Penny Dreadful", "Penny").Replace(" ", string.Empty);

            this.disabledColorIdentityIds = this.context.Colors.Where(c => c.Symbol != "C" && !settings.ColorIdentity.Contains(c.Symbol)).Select(t => t.ID).ToList();

            var disabledCardSupertypes = new List<string>();
            var disabledCardTypes = new List<string>();
            var disabledCardSubtypes = new List<string>();

            if (settings.BasicLands < 0)
            {
                disabledCardSupertypes.Add("Basic");
            }

            if (settings.NonbasicLands < 0)
            {
                disabledCardTypes.Add("land");
            }

            if (settings.Creatures < 0)
            {
                disabledCardTypes.Add("creature");
            }

            if (settings.Artifacts < 0)
            {
                disabledCardTypes.Add("artifact");
            }

            if (settings.Equipment < 0)
            {
                disabledCardSubtypes.Add("Equipment");
            }

            if (settings.Vehicles < 0)
            {
                disabledCardSubtypes.Add("Vehicle");
            }

            if (settings.Enchantments < 0)
            {
                disabledCardTypes.Add("enchantment");
            }

            if (settings.Auras < 0)
            {
                disabledCardSubtypes.Add("Aura");
            }

            if (settings.Planeswalkers < 0)
            {
                disabledCardTypes.Add("planeswalker");
            }

            if (settings.Spells < 0)
            {
                disabledCardTypes.Add("instant");
                disabledCardTypes.Add("sorcery");
            }

            if (settings.Legendary < 0)
            {
                disabledCardSupertypes.Add("Legendary");
            }

            this.disabledCardSupertypeIds = this.context.Supertypes.Where(t => disabledCardSupertypes.Contains(t.Name)).Select(t => t.ID).ToList();
            this.disabledCardTypeIds = this.context.CardTypes.Where(t => disabledCardTypes.Contains(t.Name)).Select(t => t.ID).ToList();
            this.disabledCardSubtypeIds = this.context.Subtypes.Where(t => disabledCardSubtypes.Contains(t.Name)).Select(t => t.ID).ToList();

            if (settings.SharesType < 0)
            {
                foreach (int id in this.commanderCreatureTypeIds)
                {
                    if (!this.disabledCardSubtypeIds.Contains(id))
                    {
                        this.disabledCardSubtypeIds.Add(id);
                    }
                }
            }
        }

        private IQueryable<Card> ApplyRestrictions(IQueryable<Card> cards)
        {
            foreach (int disabledCardTypeId in this.disabledCardTypeIds)
            {
                cards = cards.Where(c => !c.Types.Select(c => c.CardTypeID).Contains(disabledCardTypeId));
            }

            foreach (int disabledCardSubtypeId in this.disabledCardSubtypeIds)
            {
                cards = cards.Where(c => !c.Subtypes.Select(c => c.SubtypeID).Contains(disabledCardSubtypeId));
            }

            foreach (int disabledCardSupertypeId in this.disabledCardSupertypeIds)
            {
                cards = cards.Where(c => !c.Supertypes.Select(c => c.SupertypeID).Contains(disabledCardSupertypeId));
            }

            if (this.settings.CmcRange.Min > 0 || this.settings.CmcRange.Max < 16)
            {
                cards = cards.Where(c => c.CMC >= this.settings.CmcRange.Min && c.CMC <= this.settings.CmcRange.Max);
            }

            if (this.settings.SetIds.Length > 0)
            {
                cards = cards.Where(c => c.Printings.Any(p => this.settings.SetIds.Contains(p.SetID)));
            }

            if (this.settings.ArtistIds.Length > 0)
            {
                cards = cards.Where(c => c.Printings.Any(p => p.ArtistID.HasValue && this.settings.ArtistIds.Contains(p.ArtistID.Value)));
            }

            if (this.settings.FrameIds.Length > 0)
            {
                cards = cards.Where(c => c.Printings.Any(p => this.settings.FrameIds.Contains(p.FrameID)));
            }

            if (this.settings.EdhRecRange.Min > 0 || this.settings.EdhRecRange.Max < 100)
            {
                var ranks = cards.Where(c => c.EDHRECRank.HasValue && c.EDHRECRank > 0).Select(c => c.EDHRECRank.Value).Distinct().OrderByDescending(r => r).ToList();
                int lowerBand = ranks[Math.Max(0, this.settings.EdhRecRange.Min * ranks.Count / 100)];
                int upperBand = ranks[Math.Min(ranks.Count - 1, this.settings.EdhRecRange.Max * (ranks.Count - 1) / 100)];

                cards = cards.Where(c => c.EDHRECRank <= lowerBand && c.EDHRECRank >= upperBand);
            }

            return cards;
        }

        private void PickManaProducers(int numberToPick)
        {
            var cardPool = this.context.GetLegalCards(this.format, this.settings.SilverBorder)
                .ManaProducingFilter();

            cardPool = this.ApplyRestrictions(cardPool);

            this.AddCards(cardPool, numberToPick);
        }

        private void PickLegendary(int numberToPick)
        {
            var cardPool = this.context.GetLegalCards(this.format, this.settings.SilverBorder)
                .LegendaryFilter();

            cardPool = this.ApplyRestrictions(cardPool);

            this.AddCards(cardPool, numberToPick);
        }

        private void PickBasicLands(int numberToPick)
        {
            var basicLandNames = this.settings.ColorIdentity.Length == 0
                ? this.context.Colors.Where(c => c.Name == "Colorless").Select(c => c.BasicLandName)
                : this.context.Colors.Where(c => this.settings.ColorIdentity.Contains(c.Symbol)).Select(c => c.BasicLandName);

            int added = 0;
            var basicLands = this.context.Cards.Where(c => basicLandNames.Contains(c.Name)).IncludeCardProperties();
            while (added < numberToPick)
            {
                foreach (var land in basicLands)
                {
                    this.deck.Cards.Add(land);

                    added++;
                    if (added >= numberToPick)
                    {
                        break;
                    }
                }
            }
        }

        private void PickNonbasicLands(int numberToPick)
        {
            var cardPool = this.context.GetLegalCards(this.format, this.settings.SilverBorder)
                .NonbasicLandFilter();

            cardPool = this.ApplyRestrictions(cardPool);

            this.AddCards(cardPool, numberToPick);
        }

        private void PickCreatures(int numberToPick)
        {
            var cardPool = this.context.GetLegalCards(this.format, this.settings.SilverBorder)
                .CreatureFilter();

            cardPool = this.ApplyRestrictions(cardPool);

            if (this.settings.SharesType > 0)
            {
                var sharedTypeCardsToAdd = cardPool
                    .Where(c => c.Subtypes.Any(c => this.commanderCreatureTypeIds.Contains(c.SubtypeID)));

                int added = this.AddCards(sharedTypeCardsToAdd, this.settings.SharesType);
                numberToPick -= added;
            }

            this.AddCards(cardPool, numberToPick);
        }

        private void PickArtifacts(int numberToPick)
        {
            var cardPool = this.context.GetLegalCards(this.format, this.settings.SilverBorder)
                .ArtifactFilter();

            cardPool = this.ApplyRestrictions(cardPool);

            int equipmentToPick = this.settings.Equipment - this.deck.Equipment;
            if (equipmentToPick > 0)
            {
                var equipmentToAdd = cardPool.EquipmentFilter();

                int added = this.AddCards(equipmentToAdd, equipmentToPick);
                numberToPick -= added;
            }

            int vehiclesToPick = this.settings.Vehicles - this.deck.Vehicles;
            if (vehiclesToPick > 0)
            {
                var vehiclesToAdd = cardPool.VehicleFilter();

                int added = this.AddCards(vehiclesToAdd, vehiclesToPick);
                numberToPick -= added;
            }

            this.AddCards(cardPool, numberToPick);
        }

        private void PickEnchantments(int numberToPick)
        {
            var cardPool = this.context.GetLegalCards(this.format, this.settings.SilverBorder)
                .EnchantmentFilter();

            cardPool = this.ApplyRestrictions(cardPool);

            int aurasToPick = this.settings.Equipment - this.deck.Equipment;
            if (aurasToPick > 0)
            {
                var aurasToAdd = cardPool.AuraFilter();

                int added = this.AddCards(aurasToAdd, aurasToPick);
                numberToPick -= added;
            }

            this.AddCards(cardPool, numberToPick);
        }

        private void PickPlaneswalkers(int numberToPick)
        {
            var cardPool = this.context.GetLegalCards(this.format, this.settings.SilverBorder)
                .PlaneswalkerFilter();

            cardPool = this.ApplyRestrictions(cardPool);

            this.AddCards(cardPool, numberToPick);
        }

        private void PickSpells(int numberToPick)
        {
            var cardPool = this.context.GetLegalCards(this.format, this.settings.SilverBorder)
                .SpellFilter();

            cardPool = this.ApplyRestrictions(cardPool);

            this.AddCards(cardPool, numberToPick);
        }

        private void PickFiller(int numberToPick, bool strict = true)
        {
            var cardPool = this.context.GetLegalCards(this.format, this.settings.SilverBorder);

            foreach (int disabledColorIdentityId in this.disabledColorIdentityIds)
            {
                cardPool = cardPool.Where(c => !c.ColorIdentity.Select(c => c.ColorID).Contains(disabledColorIdentityId));
            }

            if (strict)
            {
                cardPool = this.ApplyRestrictions(cardPool);
            }

            this.AddCards(cardPool, numberToPick);
        }

        private int AddCards(IQueryable<Card> cardPool, int numberToPick)
        {
            numberToPick = Math.Min(this.settings.DeckSize - this.deck.Cards.Count, numberToPick);
            if (numberToPick <= 0)
            {
                return 0;
            }

            var cardsToAdd = cardPool
                .Where(c => (this.settings.CommanderId == null || c.ID != this.settings.CommanderId)
                         && (this.settings.PartnerId == null || c.ID != this.settings.PartnerId)
                         && (this.settings.SignatureSpellId == null || c.ID != this.settings.SignatureSpellId))
                .Where(c => !this.deck.Cards.Select(dc => (string.IsNullOrEmpty(dc.Side) || dc.Side == "a" || (dc.Layout.Name == "meld" && dc.Side == "b")) ? dc.ID : dc.MainSideID).Contains(c.ID))
                .OrderBy(c => Guid.NewGuid())
                .Take(numberToPick);

            int cardsAdded = 0;
            foreach (var card in cardsToAdd)
            {
                int copies;
                if (card.OracleText.Contains("a deck can have any number"))
                {
                    copies = this.RNG.Next(1, numberToPick - cardsAdded + 1);
                }
                else if (this.deck.Singleton)
                {
                    copies = 1;
                }
                else
                {
                    copies = this.RNG.Next(1, Math.Min(5, numberToPick - cardsAdded + 1));
                }

                for (int i = 0; i < copies; i++)
                {
                    if (string.IsNullOrEmpty(card.Side) || card.Side == "a" || (card.Layout.Name == "meld" && card.Side == "b"))
                    {
                        this.deck.Cards.Add(card);
                    }
                    else
                    {
                        this.deck.Cards.Add(card.MainSide);
                    }
                }

                cardsAdded += copies;

                if (cardsAdded >= numberToPick)
                {
                    break;
                }
            }

            return cardsAdded;
        }

        public Deck GenerateDeckAsync()
        {
            int manaProducersToPick = this.settings.ManaProducing - this.deck.ManaProducing;
            if (manaProducersToPick > 0)
            {
                this.PickManaProducers(manaProducersToPick);
            }

            int legendsToPick = this.settings.Legendary - this.deck.Legendary;
            if (legendsToPick > 0)
            {
                this.PickLegendary(legendsToPick);
            }

            if (this.settings.BasicLands > 0)
            {
                this.PickBasicLands(this.settings.BasicLands);
            }

            int landsToPick = this.settings.NonbasicLands - this.deck.NonbasicLands;
            if (landsToPick > 0)
            {
                this.PickNonbasicLands(landsToPick);
            }

            int creaturesToPick = this.settings.Creatures - this.deck.Creatures;
            if (creaturesToPick > 0)
            {
                this.PickCreatures(creaturesToPick);
            }

            int artifactsToPick = this.settings.Artifacts - this.deck.Artifacts;
            if (artifactsToPick > 0)
            {
                this.PickArtifacts(artifactsToPick);
            }

            int enchantmentsToPick = this.settings.Enchantments - this.deck.Enchantments;
            if (enchantmentsToPick > 0)
            {
                this.PickEnchantments(enchantmentsToPick);
            }

            int planeswalkersToPick = this.settings.Planeswalkers - this.deck.Planeswalkers;
            if (planeswalkersToPick > 0)
            {
                this.PickPlaneswalkers(planeswalkersToPick);
            }

            int spellsToPick = this.settings.Spells - this.deck.Spells;
            if (spellsToPick > 0)
            {
                this.PickSpells(spellsToPick);
            }

            if (this.deck.Cards.Count < this.settings.DeckSize)
            {
                this.PickFiller(this.settings.DeckSize - this.deck.Cards.Count, true);
            }

            if (this.deck.Cards.Count < this.settings.DeckSize)
            {
                this.PickFiller(this.settings.DeckSize - this.deck.Cards.Count, false);
                this.deck.Issues.AppendLine("Unable to find enough cards matching restrictions. Alternate format-legal filler has been added.");
            }

            return this.deck;
        }
    }
}