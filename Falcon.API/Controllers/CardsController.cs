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

        private readonly MTGDBContainer db = new MTGDBContainer();

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
                var filler = this.db.LegalCards(options.Format);

                foreach (var card in deck.Cards)
                {
                    filler = filler.FilterOutCard(card);
                }

                filler = filler.FilterByColorIdentity(deck.ColorIdentity).Shuffle().Take(deck.FillerNeeded - deck.Cards.Count);

                deck.Cards.AddRange(filler.ToList());
            }

            deck.Cards.AddRange(this.db.GetBasicLands(deck.BasicLandsNeeded, deck.ColorIdentity, options.Format));

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

            return this.Ok(new CardDto(c));
        }

        // GET: /api/Cards/Commanders/{format}
        [HttpGet]
        [ActionName("Commanders")]
        public dynamic GetCommanders([FromUri(Name = "key")]Format format = Format.Commander)
        {
            var cmdrs = this.db.GetCommanders(format);

            return cmdrs.OrderBy(c => c.Name).ToList()
                .Select(c => new
                {
                    value = c.ID,
                    label = c.Name,
                    hasPartner = c.HasKeyword("Partner")
                });
        }

        // POST: /api/Cards/RandomCommander
        [HttpPost]
        [ActionName("RandomCommander")]
        public dynamic RandomCommander([FromBody]RandomCommanderRequestModel request)
        {
            var cmdrs = this.db.GetCommanders(request.Format);

            if (request.MatchAll)
            {
                cmdrs = cmdrs.Where(c => c.Colors.Select(color => color.Symbol).Intersect(request.Colors).Count() == c.Colors.Count);
            }
            else
            {
                cmdrs = cmdrs.Where(c => c.Colors.Count == 0 || c.Colors.Select(color => color.Symbol).Intersect(request.Colors).Any());
            }

            var results = new List<Card>();
            var cmdr1 = cmdrs.Shuffle().FirstOrDefault();

            results.Add(cmdr1);

            if (results[0].Keywords.Select(a => a.Name).Contains("Partner"))
            {
                results.Add(cmdrs.Where(c => c.Name != cmdr1.Name && c.Keywords.Select(a => a.Name).Contains("Partner")).Shuffle().FirstOrDefault());
            }

            return results.Select(c => new
            {
                value = c.ID,
                label = c.Name,
                hasPartner = c.HasKeyword("Partner")
            });
        }

        // GET: /api/Cards/Sets
        [HttpGet]
        [ActionName("Sets")]
        public dynamic GetSets()
        {
            return this.db.Sets
                .Where(s => this.possibleSetTypes.Contains(s.SetType.Name))
                .OrderByDescending(s => s.Date)
                .Select(s => new
                {
                    value = s.Code,
                    label = s.Name
                });
        }

        // GET: /api/Cards/RandomFlavor
        [HttpGet]
        [ActionName("RandomFlavor")]
        public dynamic RandomFlavor()
        {
            return (from p in this.db.Printings
                    where !string.IsNullOrEmpty(p.FlavorText)
                    orderby Guid.NewGuid()
                    select new
                    {
                        name = p.Card.Name,
                        flavor = p.FlavorText
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