namespace Falcon.API.Core.Controllers
{
    using Falcon.API.DTO;
    using Falcon.MtG;
    using Falcon.MtG.Models;
    using Falcon.MtG.Utility;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class MtGController : ControllerBase
    {
        private readonly string[] SetTypes = new string[] { "core", "expansion", "masters", "planechase", "archenemy", "commander", "draft_innovation" };
        private readonly string[] Rarities = new string[] { "common", "uncommon", "rare", "mythic" };
        private readonly MtGDBContext context;

        public MtGController(MtGDBContext context)
        {
            this.context = context;
        }

        [HttpGet("Card/{id}")]
        public async Task<CardDto> GetCard(int id) => await context.Cards
            .Where(c => c.ID == id)
            .IncludeCardProperties()
            .Select(c => new CardDto(c))
            .SingleOrDefaultAsync();

        [HttpGet("RandomAppName")]
        public async Task<string> GetAppName() => await context.AlsoKnownAs
            .OrderBy(a => Guid.NewGuid())
            .Select(a => a.Name)
            .FirstOrDefaultAsync();

        [HttpGet("Commanders")]
        public async Task<IEnumerable<CardDto>> GetCommanders(string variant = "Commander", bool allowSilver = false) => await context.Legalities
            .Where(l => l.Format == variant.Replace(" ", string.Empty) && l.LegalAsCommander
                     && (l.Legal
                      || allowSilver && (l.Card.Printings.All(p => p.Set.SetType.Name == "funny")
                                        || !l.Card.Printings.Any())))
            .IncludeCardProperties()
            .OrderBy(l => l.Card.Name)
            .Select(l => new CardDto(l.Card))
            .ToListAsync();

        [HttpGet("Partners")]
        public async Task<IEnumerable<CardDto>> GetPartners(int cmdrId, string variant = "Commander", bool allowSilver = false)
        {
            var cmdr = await context.Cards.SingleAsync(c => c.ID == cmdrId);

            if (cmdr.OracleText.Contains("Partner"))
            {
                var legalities = context.Legalities
                .Where(l => l.Format == variant.Replace(" ", string.Empty) && l.CardID != cmdrId && l.LegalAsCommander
                         && (l.Legal
                      || allowSilver && (l.Card.Printings.All(p => p.Set.SetType.Name == "funny")
                                        || !l.Card.Printings.Any())));

                if (cmdr.OracleText.Contains("Partner with"))
                {
                    var match = Regex.Match(cmdr.OracleText, @"Partner with ([\w, ]*)( \(|$)", RegexOptions.Multiline);
                    if (match.Success && match.Groups.Count > 1)
                    {
                        string partnerName = match.Groups[1].Value;
                        if (!string.IsNullOrEmpty(partnerName))
                        {
                            legalities = legalities.Where(l => l.Card.Name == partnerName);
                        }
                    }
                }
                else
                {
                    legalities = legalities.Where(l => l.Card.OracleText.Contains("Partner") && !l.Card.OracleText.Contains("Partner with"));
                }

                return await legalities
                .IncludeCardProperties()
                .OrderBy(l => l.Card.Name)
                .Select(l => new CardDto(l.Card))
                .ToListAsync();
            }
            else if (cmdr.OracleText.Contains("Choose a Background"))
            {
                var legalities = context.Legalities
                .Where(l => l.Format == variant.Replace(" ", string.Empty) && l.CardID != cmdrId && l.Card.TypeLine.Contains("Legendary") && l.Card.TypeLine.Contains("Enchantment") && l.Card.TypeLine.Contains("Background")
                         && (l.Legal
                      || allowSilver && (l.Card.Printings.All(p => p.Set.SetType.Name == "funny")
                                        || !l.Card.Printings.Any())));

                return await legalities
                .IncludeCardProperties()
                .OrderBy(l => l.Card.Name)
                .Select(l => new CardDto(l.Card))
                .ToListAsync();
            }
            else
            {
                return new List<CardDto>();
            }
        }

        [HttpGet("SignatureSpells")]
        public async Task<IEnumerable<CardDto>> GetSignatureSpells(int obId, bool allowSilver = false)
        {
            var colorIdentity = context.CardColorIdentities.Where(c => c.CardID == obId)
                .Include(c => c.Color).Select(c => c.Color.Symbol);

            return await context
            .GetLegalCards("Oathbreaker", allowSilver)
            .Where(c =>
                c.Types.Any(t => t.CardType.Name == "instant" || t.CardType.Name == "sorcery")
                && (string.IsNullOrEmpty(c.Side) || c.Side == "a")
                && !c.ColorIdentity.Select(ci => ci.Color.Symbol).Except(colorIdentity).Any())
            .OrderBy(c => c.Name)
            .Select(c => new CardDto(c))
            .ToListAsync();
        }

        [HttpGet("RandomFlavor")]
        public async Task<FlavorDto> GetRandomFlavorText() => await context.Printings
            .Where(p => !string.IsNullOrEmpty(p.FlavorText))
            .OrderBy(p => Guid.NewGuid())
            .Select(p => new FlavorDto
            {
                FlavorText = p.FlavorText,
                Name = p.Card.Name
            })
            .FirstAsync();

        [HttpGet("Sets")]
        public async Task<IEnumerable<SetDto>> GetSets(string variant = "Commander", bool allowSilver = false) => await context
            .GetLegalCards(variant, allowSilver)
            .SelectMany(c => c.Printings)
            .Select(p => p.Set)
            .Where(s => SetTypes.Contains(s.SetType.Name) || (allowSilver && s.SetType.Name == "funny"))
            .Distinct()
            .OrderByDescending(s => s.Date)
            .Select(s => new SetDto(s))
            .ToListAsync();

        [HttpGet("Watermarks")]
        public async Task<IEnumerable<KeyValueDto>> GetWatermarks() => await context.Watermarks
            .OrderBy(w => w.Name)
            .Select(w => new KeyValueDto
            {
                ID = w.ID,
                Name = w.Name
            })
            .ToListAsync();

        [HttpGet("Rarities")]
        public async Task<IEnumerable<KeyValueDto>> GetRarities() => await context.Rarities
            .Where(r => Rarities.Contains(r.Name))
            .OrderBy(r => r.Name == "common" ? 1 : r.Name == "uncommon" ? 2 : r.Name == "rare" ? 3 : r.Name == "mythic" ? 4 : 5)
            .Select(r => new KeyValueDto
            {
                ID = r.ID,
                Name = r.Name
            })
            .ToListAsync();

        [HttpGet("Frames")]
        public async Task<IEnumerable<KeyValueDto>> GetFrames() => await context.Frames
            .OrderBy(f => f.Name)
            .Select(f => new KeyValueDto
            {
                ID = f.ID,
                Name = f.Name
            })
            .ToListAsync();

        [HttpGet("Layouts")]
        public async Task<IEnumerable<KeyValueDto>> GetLayouts() => await context.Layouts
            .OrderBy(l => l.Name)
            .Select(l => new KeyValueDto
            {
                ID = l.ID,
                Name = l.Name
            })
            .ToListAsync();

        [HttpGet("Artists")]
        public async Task<IEnumerable<KeyValueDto>> GetArtists() => await context.Artists
            .OrderBy(a => a.Name)
            .Select(a => new KeyValueDto
            {
                ID = a.ID,
                Name = a.Name
            })
            .ToListAsync();

        [HttpPost("GenerateDeck")]
        public DeckResponseDto GenerateDeck([FromBody] GenerateDeckDto dto)
        {
            DeckGenerator generator = new(context, dto);
            Deck deck = generator.GenerateDeck();

            return new DeckResponseDto(deck);
        }
    }
}