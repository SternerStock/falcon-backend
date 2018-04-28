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
            EDHDeck deck;

            Card commander1 = this.db.Cards.Find(options.Commander1Id);
            if (options.Commander2Id != null)
            {
                Card commander2 = this.db.Cards.Find(options.Commander2Id);
                deck = new EDHDeck(options, commander1, commander2);
            }
            else
            {
                deck = new EDHDeck(options, commander1);
            }

            foreach (var category in options.Categories)
            {
                if (!category.Enabled)
                {
                    category.Count = 0;
                }
            }

            File.WriteAllText(HostingEnvironment.MapPath(AnalyticsFolder + "/" + Guid.NewGuid().ToString("D") + ".json"), JsonConvert.SerializeObject(options));

            Library library = new Library(this.db, options.Format);
            library.FilterSets(options.SetCodes);
            library.FilterByCmc(options.CMC);

            library.FilterNonbasicLands(options.NonbasicLands);
            library.FilterCreatures(options.Creatures);
            library.FilterArtifacts(options.Artifacts, options.Equipment);
            library.FilterEnchantments(options.Enchantments, options.Auras);
            library.FilterPlaneswalkers(options.Planeswalkers);
            library.FilterSpells(options.Spells);
            library.FilterManaProducers(options.ManaProducing, options.ManaProducing.Count >= deck.ReqDeckSize);
            library.FilterLegendary(options.Legendary, options.Legendary.Count >= deck.ReqDeckSize);

            var cmdrIds = deck.Commanders.Select(cmdr => cmdr.ID);
            var legalCards = library.NonlandCards.Where(c => !cmdrIds.Contains(c.ID)).FilterByColorIdentity(deck.ColorIdentity).Shuffle();

            if (options.NonbasicLands.Enabled)
            {
                var legalLands = library.LandCards.Where(c => !cmdrIds.Contains(c.ID)).FilterByColorIdentity(deck.ColorIdentity).Shuffle();
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

                filler = filler.FilterByColorIdentity(deck.ColorIdentity).Shuffle().Take(deck.FillerNeeded - deck.Cards.Count);

                deck.Cards.AddRange(filler.ToList());
            }

            deck.Cards.AddRange(this.db.GetBasicLands(deck.BasicLandsNeeded, deck.ColorIdentity));

            string message = string.Empty;

            if (!deck.LandMinimumMet || !deck.NonlandMinimumsMet)
            {
                message = "Deck does not meet all specified minimums, but is still legal.";
            }

            return new
            {
                message,
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

        // GET: /api/Cards/Commanders/{format}
        [HttpGet]
        [ActionName("Commanders")]
        public List<ResponseCard> GetCommanders([FromUri(Name = "key")]EdhFormat format = EdhFormat.Commander)
        {
            var types = new List<string>() { "Creature" };

            switch (format)
            {
                case EdhFormat.Brawl:
                    types.Add("Planeswalker");

                    return this.db.BrawlLegalCards()
                     .Where(c => (types.Any(s => c.Types.Select(t => t.Name).Contains(s)) && c.Supertypes.Select(t => t.Name).Contains("Legendary")) ||
                                c.OracleText.Contains("can be your commander."))
                     .OrderBy(c => c.Name)
                     .ToList()
                     .Select(c => new ResponseCard(c)).ToList();

                case EdhFormat.TinyLeaders:
                    types.Add("Planeswalker");

                    return this.db.TinyLeadersLegalCards()
                             .Where(c => c.TinyLeadersCmdrLegal &&
                                         ((types.Any(s => c.Types.Select(t => t.Name).Contains(s)) && c.Supertypes.Select(t => t.Name).Contains("Legendary")) ||
                                           c.OracleText.Contains("can be your commander.")))
                             .OrderBy(c => c.Name)
                             .ToList()
                             .Select(c => new ResponseCard(c)).ToList();

                case EdhFormat.Pauper:
                    return this.db.CommanderLegalCards()
                             .Where(c => (types.Any(s => c.Types.Select(t => t.Name).Contains(s)) || c.OracleText.Contains("can be your commander.")) &&
                                         c.Rarities.Select(r => r.Name).Contains("Uncommon"))
                             .OrderBy(c => c.Name)
                             .ToList()
                             .Select(c => new ResponseCard(c)).ToList();

                case EdhFormat.Commander:
                default:
                    return this.db.CommanderLegalCards()
                             .Where(c => (types.Any(s => c.Types.Select(t => t.Name).Contains(s)) && c.Supertypes.Select(t => t.Name).Contains("Legendary")) ||
                                         c.OracleText.Contains("can be your commander."))
                             .OrderBy(c => c.Name)
                             .ToList()
                             .Select(c => new ResponseCard(c)).ToList();
            }
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