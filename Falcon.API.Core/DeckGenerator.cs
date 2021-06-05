namespace Falcon.API
{
    using Falcon.API.DTO;
    using Falcon.MtG;
    using Falcon.MtG.Models;
    using Falcon.MtG.Models.Sql;
    using Falcon.MtG.Utility;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            RNG = new Random(seed);

            this.settings = settings;

            if (settings.DeckType == "Commander")
            {
                Card commander = context.GetCardById(settings.CommanderId.Value);
                Card partner = null;
                if (settings.PartnerId.HasValue)
                {
                    partner = context.GetCardById(settings.PartnerId.Value);
                }

                commanderCreatureTypeIds = new List<int>();
                if (commander?.OracleText?.Contains("is every creature type") != true && partner?.OracleText?.Contains("is every creature type") != true)
                {
                    var creatureTypes = context.Subtypes
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

                    commanderCreatureTypeIds.AddRange(creatureTypes.Select(st => st.ID));
                }

                deck = new EdhDeck()
                {
                    Commander = commander,
                    Partner = partner,
                    Singleton = true,
                    Seed = seed
                };
            }
            else if (settings.DeckType == "Oathbreaker")
            {
                deck = new OathbreakerDeck()
                {
                    Oathbreaker = context.GetCardById(settings.CommanderId.Value),
                    SignatureSpell = context.GetCardById(settings.SignatureSpellId.Value),
                    Singleton = true,
                    Seed = seed
                };
            }
            else
            {
                deck = new Deck()
                {
                    Seed = seed
                };
            }

            format = settings.Format.Replace("Penny Dreadful", "Penny").Replace(" ", string.Empty);

            disabledColorIdentityIds = context.Colors.Where(c => c.Symbol != "C" && !settings.ColorIdentity.Contains(c.Symbol)).Select(t => t.ID).ToList();

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

            disabledCardSupertypeIds = context.Supertypes.Where(t => disabledCardSupertypes.Contains(t.Name)).Select(t => t.ID).ToList();
            disabledCardTypeIds = context.CardTypes.Where(t => disabledCardTypes.Contains(t.Name)).Select(t => t.ID).ToList();
            disabledCardSubtypeIds = context.Subtypes.Where(t => disabledCardSubtypes.Contains(t.Name)).Select(t => t.ID).ToList();

            if (settings.SharesType < 0)
            {
                foreach (int id in commanderCreatureTypeIds)
                {
                    if (!disabledCardSubtypeIds.Contains(id))
                    {
                        disabledCardSubtypeIds.Add(id);
                    }
                }
            }
        }

        private IQueryable<Card> ApplyRestrictions(IQueryable<Card> cards)
        {
            foreach (int disabledColorIdentityId in disabledColorIdentityIds)
            {
                cards = cards.Where(c => !c.ColorIdentity.Select(c => c.ColorID).Contains(disabledColorIdentityId));
            }

            foreach (int disabledCardTypeId in disabledCardTypeIds)
            {
                cards = cards.Where(c => !c.Types.Select(c => c.CardTypeID).Contains(disabledCardTypeId));
            }

            foreach (int disabledCardSubtypeId in disabledCardSubtypeIds)
            {
                cards = cards.Where(c => !c.Subtypes.Select(c => c.SubtypeID).Contains(disabledCardSubtypeId));
            }

            foreach (int disabledCardSupertypeId in disabledCardSupertypeIds)
            {
                cards = cards.Where(c => !c.Supertypes.Select(c => c.SupertypeID).Contains(disabledCardSupertypeId));
            }

            if (settings.CmcRange.Min > 0 || settings.CmcRange.Max < 16)
            {
                cards = cards.Where(c => c.CMC >= settings.CmcRange.Min && c.CMC <= settings.CmcRange.Max);
            }

            if (settings.SetIds.Length > 0)
            {
                cards = cards.Where(c => c.Printings.Any(p => settings.SetIds.Contains(p.SetID)));
            }

            if (settings.RarityIds.Length > 0)
            {
                cards = cards.Where(c => c.Printings.Any(p => settings.RarityIds.Contains(p.RarityID)));
            }

            if (settings.ArtistIds.Length > 0)
            {
                cards = cards.Where(c => c.Printings.Any(p => p.ArtistID.HasValue && settings.ArtistIds.Contains(p.ArtistID.Value)));
            }

            if (settings.FrameIds.Length > 0)
            {
                cards = cards.Where(c => c.Printings.Any(p => settings.FrameIds.Contains(p.FrameID)));
            }

            if (settings.EdhRecRange.Min > 0 || settings.EdhRecRange.Max < 100)
            {
                var ranks = cards.Where(c => c.EDHRECRank.HasValue && c.EDHRECRank > 0).Select(c => c.EDHRECRank.Value).Distinct().OrderByDescending(r => r).ToList();
                int lowerBand = ranks[Math.Max(0, settings.EdhRecRange.Min * ranks.Count / 100)];
                int upperBand = ranks[Math.Min(ranks.Count - 1, settings.EdhRecRange.Max * (ranks.Count - 1) / 100)];

                cards = cards.Where(c => c.EDHRECRank <= lowerBand && c.EDHRECRank >= upperBand);
            }

            return cards;
        }

        private void PickManaProducers(int numberToPick)
        {
            var cardPool = context.GetLegalCards(format, settings.SilverBorder)
                .ManaProducingFilter();

            cardPool = ApplyRestrictions(cardPool);

            AddCards(cardPool, numberToPick);
        }

        private void PickLegendary(int numberToPick)
        {
            var cardPool = context.GetLegalCards(format, settings.SilverBorder)
                .LegendaryFilter();

            cardPool = ApplyRestrictions(cardPool);

            AddCards(cardPool, numberToPick);
        }

        private void PickBasicLands(int numberToPick)
        {
            var basicLandNames = settings.ColorIdentity.Length == 0
                ? context.Colors.Where(c => c.Name == "Colorless").Select(c => c.BasicLandName)
                : context.Colors.Where(c => settings.ColorIdentity.Contains(c.Symbol)).Select(c => c.BasicLandName);

            int added = 0;
            var basicLands = context.Cards.Where(c => basicLandNames.Contains(c.Name)).IncludeCardProperties();
            while (added < numberToPick)
            {
                foreach (var land in basicLands)
                {
                    deck.Cards.Add(land);

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
            var cardPool = context.GetLegalCards(format, settings.SilverBorder)
                .NonbasicLandFilter();

            cardPool = ApplyRestrictions(cardPool);

            AddCards(cardPool, numberToPick);
        }

        private void PickCreatures(int numberToPick)
        {
            var cardPool = context.GetLegalCards(format, settings.SilverBorder)
                .CreatureFilter();

            cardPool = ApplyRestrictions(cardPool);

            if (settings.SharesType > 0)
            {
                var sharedTypeCardsToAdd = cardPool
                    .Where(c => c.Subtypes.Any(c => commanderCreatureTypeIds.Contains(c.SubtypeID)));

                int added = AddCards(sharedTypeCardsToAdd, settings.SharesType);
                numberToPick -= added;
            }

            AddCards(cardPool, numberToPick);
        }

        private void PickArtifacts(int numberToPick)
        {
            var cardPool = context.GetLegalCards(format, settings.SilverBorder)
                .ArtifactFilter();

            cardPool = ApplyRestrictions(cardPool);

            int equipmentToPick = settings.Equipment - deck.Equipment;
            if (equipmentToPick > 0)
            {
                var equipmentToAdd = cardPool.EquipmentFilter();

                int added = AddCards(equipmentToAdd, equipmentToPick);
                numberToPick -= added;
            }

            int vehiclesToPick = settings.Vehicles - deck.Vehicles;
            if (vehiclesToPick > 0)
            {
                var vehiclesToAdd = cardPool.VehicleFilter();

                int added = AddCards(vehiclesToAdd, vehiclesToPick);
                numberToPick -= added;
            }

            AddCards(cardPool, numberToPick);
        }

        private void PickEnchantments(int numberToPick)
        {
            var cardPool = context.GetLegalCards(format, settings.SilverBorder)
                .EnchantmentFilter();

            cardPool = ApplyRestrictions(cardPool);

            int aurasToPick = settings.Equipment - deck.Equipment;
            if (aurasToPick > 0)
            {
                var aurasToAdd = cardPool.AuraFilter();

                int added = AddCards(aurasToAdd, aurasToPick);
                numberToPick -= added;
            }

            AddCards(cardPool, numberToPick);
        }

        private void PickPlaneswalkers(int numberToPick)
        {
            var cardPool = context.GetLegalCards(format, settings.SilverBorder)
                .PlaneswalkerFilter();

            cardPool = ApplyRestrictions(cardPool);

            AddCards(cardPool, numberToPick);
        }

        private void PickSpells(int numberToPick)
        {
            var cardPool = context.GetLegalCards(format, settings.SilverBorder)
                .SpellFilter();

            cardPool = ApplyRestrictions(cardPool);

            AddCards(cardPool, numberToPick);
        }

        private void PickFiller(int numberToPick, bool strict = true)
        {
            var cardPool = context.GetLegalCards(format, settings.SilverBorder);

            if (strict)
            {
                cardPool = ApplyRestrictions(cardPool);
            }
            else
            {
                foreach (int disabledColorIdentityId in disabledColorIdentityIds)
                {
                    cardPool = cardPool.Where(c => !c.ColorIdentity.Select(c => c.ColorID).Contains(disabledColorIdentityId));
                }
            }

            AddCards(cardPool, numberToPick);
        }

        private int AddCards(IQueryable<Card> cardPool, int numberToPick)
        {
            numberToPick = Math.Min(settings.DeckSize - deck.Cards.Count, numberToPick);
            if (numberToPick <= 0)
            {
                return 0;
            }

            var cardsToAdd = cardPool
                .Where(c => (settings.CommanderId == null || c.ID != settings.CommanderId)
                         && (settings.PartnerId == null || c.ID != settings.PartnerId)
                         && (settings.SignatureSpellId == null || c.ID != settings.SignatureSpellId))
                .Where(c => !deck.Cards.Select(dc => (string.IsNullOrEmpty(dc.Side) || dc.Side == "a" || (dc.Layout.Name == "meld" && dc.Side == "b")) ? dc.ID : dc.MainSideID).Contains(c.ID))
                .OrderBy(c => Guid.NewGuid())
                .Take(numberToPick);

            int cardsAdded = 0;
            foreach (var card in cardsToAdd)
            {
                int copies;
                if (card.OracleText.Contains("a deck can have any number"))
                {
                    copies = RNG.Next(1, numberToPick - cardsAdded + 1);
                }
                else if (deck.Singleton)
                {
                    copies = 1;
                }
                else
                {
                    copies = RNG.Next(1, Math.Min(5, numberToPick - cardsAdded + 1));
                }

                for (int i = 0; i < copies; i++)
                {
                    if (card.MainSide == null || string.IsNullOrEmpty(card.Side) || card.Side == "a" || (card.Layout.Name == "meld" && card.Side == "b"))
                    {
                        deck.Cards.Add(card);
                    }
                    else
                    {
                        deck.Cards.Add(card.MainSide);
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

        public Deck GenerateDeck()
        {
            int manaProducersToPick = settings.ManaProducing - deck.ManaProducing;
            if (manaProducersToPick > 0)
            {
                PickManaProducers(manaProducersToPick);
            }

            int legendsToPick = settings.Legendary - deck.Legendary;
            if (legendsToPick > 0)
            {
                PickLegendary(legendsToPick);
            }

            if (settings.BasicLands > 0)
            {
                PickBasicLands(settings.BasicLands);
            }

            int landsToPick = settings.NonbasicLands - deck.NonbasicLands;
            if (landsToPick > 0)
            {
                PickNonbasicLands(landsToPick);
            }

            int creaturesToPick = settings.Creatures - deck.Creatures;
            if (creaturesToPick > 0)
            {
                PickCreatures(creaturesToPick);
            }

            int artifactsToPick = settings.Artifacts - deck.Artifacts;
            if (artifactsToPick > 0)
            {
                PickArtifacts(artifactsToPick);
            }

            int enchantmentsToPick = settings.Enchantments - deck.Enchantments;
            if (enchantmentsToPick > 0)
            {
                PickEnchantments(enchantmentsToPick);
            }

            int planeswalkersToPick = settings.Planeswalkers - deck.Planeswalkers;
            if (planeswalkersToPick > 0)
            {
                PickPlaneswalkers(planeswalkersToPick);
            }

            int spellsToPick = settings.Spells - deck.Spells;
            if (spellsToPick > 0)
            {
                PickSpells(spellsToPick);
            }

            if (deck.Cards.Count < settings.DeckSize)
            {
                PickFiller(settings.DeckSize - deck.Cards.Count, true);
            }

            if (deck.Cards.Count < settings.DeckSize)
            {
                PickFiller(settings.DeckSize - deck.Cards.Count, false);
                deck.Issues.AppendLine("Unable to find enough cards matching restrictions. Alternate format-legal filler has been added.");
            }

            return deck;
        }
    }
}