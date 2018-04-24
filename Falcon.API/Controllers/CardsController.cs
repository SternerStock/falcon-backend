namespace Falcon.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Falcon.API.Helpers;
    using Falcon.API.Models;
    using Falcon.MtG;
    using Newtonsoft.Json;

    public class CardsController : ApiController
    {
        private const string AnalyticsFolder = "~/App_Data/Analytics";

        private readonly string[] possibleSetTypes =
        {
            "core", "expansion", "commander", "planechase", "archenemy", "masters", "conspiracy", "reprint"
        };

        private MTGDBContainer db = new MTGDBContainer();

        // POST: /api/Cards/GenerateDeck
        [HttpPost]
        [ActionName("GenerateDeck")]
        public dynamic GenerateDeck([FromBody]GeneratorRequestModel options)
        {
            Card commander = this.db.Cards.Find(options.CommanderId);
            var deck = new EDHDeck(commander, options);

            foreach (var category in options.Categories)
            {
                if (!category.Enabled)
                {
                    category.Count = 0;
                }
            }

            File.WriteAllText(HostingEnvironment.MapPath(AnalyticsFolder + "/" + Guid.NewGuid().ToString("D") + ".json"), JsonConvert.SerializeObject(options));

            Library library = new Library(this.db);
            library.FilterByVariant(options.Variant);
            library.FilterSets(options.SetCodes);
            library.FilterByCmc(options.CMC);

            library.FilterNonbasicLands(options.NonbasicLands);
            library.FilterCreatures(options.Creatures);
            library.FilterArtifacts(options.Artifacts, options.Equipment);
            library.FilterEnchantments(options.Enchantments, options.Auras);
            library.FilterPlaneswalkers(options.Planeswalkers);
            library.FilterSpells(options.Spells);
            library.FilterManaProducers(options.ManaProducing);
            library.FilterLegendary(options.Legendary);

            var legalCards = library.NonlandCards.FilterOutCard(commander).FilterByColorIdentity(commander.ColorIdentity).Shuffle();

            if (options.NonbasicLands.Enabled)
            {
                var legalLands = library.LandCards.FilterOutCard(commander).FilterByColorIdentity(commander.ColorIdentity).Shuffle();
                if (!deck.LandMinimumMet)
                {
                    foreach (Card candidate in legalLands)
                    {
                        deck.AddCard(candidate);

                        if (deck.LandMinimumMet)
                        {
                            break;
                        }
                    }
                }
            }

            int passes = 0;

            var nonlandCategories = options.Categories.Where(c => !c.Name.Contains("Lands"));

            while (!deck.IsFullExceptForBasicLands && passes < 2)
            {
                foreach (Card candidate in legalCards)
                {
                    if (deck.IsFullExceptForBasicLands)
                    {
                        break;
                    }

                    if (deck.Cards.Contains(candidate))
                    {
                        continue;
                    }

                    deck.AddCard(candidate, passes > 0);
                }

                passes++;
            }

            if (!deck.IsFullExceptForBasicLands)
            {
                var filler = this.db.CommanderLegalCards();

                foreach (var card in deck.Cards)
                {
                    filler = filler.FilterOutCard(card);
                }

                filler = filler.FilterByColorIdentity(commander.ColorIdentity).Shuffle().Take(deck.FillerNeeded - deck.Cards.Count);

                deck.Cards.AddRange(filler.ToList());
            }

            deck.Cards.AddRange(this.db.GetBasicLands(deck.BasicLandsNeeded, commander.ColorIdentity));

            string message = string.Empty;

            if (!deck.LandMinimumMet || !deck.NonlandMinimumsMet)
            {
                message = "Deck does not meet all specified minimums, but is still legal.";
            }

            return new
            {
                message = message,
                deck = deck.ToString()
            };
        }

        // GET: /api/Cards/5
        [ResponseType(typeof(Card))]
        public async Task<IHttpActionResult> Get(int id)
        {
            Card c = await this.db.Cards.FindAsync(id);
            if (c == null)
            {
                return this.NotFound();
            }

            return this.Ok(new ResponseCard(c));
        }

        // GET: /api/Cards
        public List<ResponseCard> GetCards()
        {
            return this.db.Cards.OrderBy(c => c.Name).ToList()
                           .Select(c => new ResponseCard(c)).ToList();
        }

        // GET: /api/Cards/PauperCommanders
        [HttpGet]
        [ActionName("PauperCommanders")]
        public List<ResponseCard> GetPauperCommanders()
        {
            return this.db.CommanderLegalCards()
                     .Where(c => (c.Types.Select(t => t.Name).Contains("Creature") || c.OracleText.Contains("can be your commander.")) &&
                                 c.Rarities.Select(r => r.Name).Contains("Uncommon"))
                     .OrderBy(c => c.Name)
                     .ToList()
                     .Select(c => new ResponseCard(c)).ToList();
        }

        // GET: /api/Cards/Sets
        [HttpGet]
        [ActionName("Sets")]
        public List<ResponseSet> GetSets()
        {
            return this.db.Sets
                     .Where(s => this.possibleSetTypes.Contains(s.Type) && s.Border != "silver")
                     .OrderByDescending(s => s.Date)
                     .ToList()
                     .Select(s => new ResponseSet(s)).ToList();
        }

        // GET: /api/Cards/Commanders
        [HttpGet]
        [ActionName("Commanders")]
        public List<ResponseCard> GetStandardCommanders()
        {
            return this.db.CommanderLegalCards()
                     .Where(c => (c.Types.Select(t => t.Name).Contains("Creature") && c.Supertypes.Select(t => t.Name).Contains("Legendary")) ||
                                 c.OracleText.Contains("can be your commander."))
                     .OrderBy(c => c.Name)
                     .ToList()
                     .Select(c => new ResponseCard(c)).ToList();
        }

        // GET: /api/Cards/RandomFlavor
        [HttpGet]
        [ActionName("RandomFlavor")]
        public dynamic RandomFlavor()
        {
            return (from c in this.db.Cards
                    where !string.IsNullOrEmpty(c.FlavorText)
                    orderby Guid.NewGuid()
                    select new
                    {
                        c.Name,
                        c.FlavorText
                    }).FirstOrDefault();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}