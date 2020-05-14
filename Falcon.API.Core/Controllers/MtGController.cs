namespace Falcon.API.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Falcon.API.DTO;
    using Falcon.MtG;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class MtGController : ControllerBase
    {
        private readonly MtGDBContext context;

        public MtGController(MtGDBContext context)
        {
            this.context = context;
        }

        [HttpGet("Card/{id}")]
        public async Task<CardDto> GetCard(int id) => await this.context.Cards
            .Where(c => c.ID == id)
            .Include(c => c.Colors)
            .Include(c => c.ColorIdentity)
                .ThenInclude(c => c.Color)
            .Include(c => c.Printings)
                .ThenInclude(p => p.Artist)
            .Include(c => c.Printings)
                .ThenInclude(p => p.Watermark)
            .Include(c => c.Printings)
                .ThenInclude(p => p.Set)
            .Select(c => new CardDto(c))
            .SingleOrDefaultAsync();

        [HttpGet("Commanders")]
        public async Task<List<CardDto>> GetCommanders(string variant = "Commander") => await this.context.Legalities
            .Where(l => l.Format == variant.Replace(" ", string.Empty) && l.LegalAsCommander)
            .Include(c => c.Card)
                .ThenInclude(c => c.Colors)
            .Include(c => c.Card)
                .ThenInclude(c => c.ColorIdentity)
                    .ThenInclude(c => c.Color)
            .Include(c => c.Card)
                .ThenInclude(c => c.Printings)
                    .ThenInclude(p => p.Artist)
            .Include(c => c.Card)
                .ThenInclude(c => c.Printings)
                    .ThenInclude(p => p.Watermark)
            .Include(c => c.Card)
                .ThenInclude(c => c.Printings)
                    .ThenInclude(p => p.Set)
            .OrderBy(l => l.Card.Name)
            .Select(l => new CardDto(l.Card))
            .ToListAsync();

        [HttpGet("Partners")]
        public async Task<List<CardDto>> GetPartners(int cmdrId, string variant = "Commander")
        {
            var cmdr = await this.context.Cards.SingleAsync(c => c.ID == cmdrId);
            if (!cmdr.OracleText.Contains("Partner"))
            {
                return new List<CardDto>();
            }

            var cards = this.context.Legalities.Where(l => l.Format == variant && l.LegalAsCommander);

            if (cmdr.OracleText.Contains("Partner with"))
            {
                var match = Regex.Match(cmdr.OracleText, @"Partner with ([\w, ]*)( \(|$)");
                if (match.Success && match.Groups.Count > 1)
                {
                    string partnerName = match.Groups[1].Value;
                    if (!string.IsNullOrEmpty(partnerName))
                    {
                        cards = cards.Where(c => c.Card.Name == partnerName);
                    }
                }
            }
            else
            {
                cards = cards.Where(c => c.Card.OracleText.Contains("Partner") && !c.Card.OracleText.Contains("Partner with"));
            }

            return await cards
            .Include(c => c.Card)
                .ThenInclude(c => c.Colors)
            .Include(c => c.Card)
                .ThenInclude(c => c.ColorIdentity)
                    .ThenInclude(c => c.Color)
            .Include(c => c.Card)
                .ThenInclude(c => c.Printings)
                    .ThenInclude(p => p.Artist)
            .Include(c => c.Card)
                .ThenInclude(c => c.Printings)
                    .ThenInclude(p => p.Watermark)
            .Include(c => c.Card)
                .ThenInclude(c => c.Printings)
                    .ThenInclude(p => p.Set)
            .OrderBy(l => l.Card.Name)
            .Select(l => new CardDto(l.Card))
            .ToListAsync();
        }

        [HttpGet("SignatureSpells")]
        public async Task<List<CardDto>> GetSignatureSpells(int obId)
        {
            var colorIdentity = this.context.CardColorIdentities.Where(c => c.CardID == obId)
                .Include(c => c.Color).Select(c => c.Color.Symbol);

            return await this.context.Legalities
            .Where(l => l.Format == "Oathbreaker" && l.Legal && l.Card.Types.Any(t => t.CardType.Name == "instant" || t.CardType.Name == "sorcery") && l.Card.ColorIdentity.Select(ci => ci.Color.Symbol).Intersect(colorIdentity).Any())
            .Include(c => c.Card)
                .ThenInclude(c => c.Colors)
            .Include(c => c.Card)
                .ThenInclude(c => c.ColorIdentity)
                    .ThenInclude(c => c.Color)
            .Include(c => c.Card)
                .ThenInclude(c => c.Printings)
                    .ThenInclude(p => p.Artist)
            .Include(c => c.Card)
                .ThenInclude(c => c.Printings)
                    .ThenInclude(p => p.Watermark)
            .Include(c => c.Card)
                .ThenInclude(c => c.Printings)
                    .ThenInclude(p => p.Set)
            .OrderBy(l => l.Card.Name)
            .Select(l => new CardDto(l.Card))
            .ToListAsync();
        }

        [HttpGet("RandomFlavor")]
        public async Task<FlavorDto> GetRandomFlavorText() => await this.context.Printings
            .Where(p => !string.IsNullOrEmpty(p.FlavorText))
            .OrderBy(p => Guid.NewGuid())
            .Select(p => new FlavorDto
            {
                FlavorText = p.FlavorText,
                Name = p.Card.Name
            })
            .FirstAsync();

        [HttpGet("Sets")]
        public async Task<List<SetDto>> GetSets() => await this.context.Sets
            .OrderByDescending(s => s.Date)
            .Select(s => new SetDto(s))
            .ToListAsync();

        [HttpGet("Watermarks")]
        public async Task<List<KeyValueDto>> GetWatermarks() => await this.context.Watermarks
            .Select(w => new KeyValueDto
            {
                ID = w.ID,
                Name = w.Name
            })
            .ToListAsync();

        [HttpGet("Rarities")]
        public async Task<List<KeyValueDto>> GetRarities() => await this.context.Rarities
            .Select(r => new KeyValueDto
            {
                ID = r.ID,
                Name = r.Name
            })
            .ToListAsync();

        [HttpGet("Frames")]
        public async Task<List<KeyValueDto>> GetFrames() => await this.context.Frames
            .Select(r => new KeyValueDto
            {
                ID = r.ID,
                Name = r.Name
            })
            .ToListAsync();

        [HttpGet("Layouts")]
        public async Task<List<KeyValueDto>> GetLayouts() => await this.context.Layouts
            .Select(r => new KeyValueDto
            {
                ID = r.ID,
                Name = r.Name
            })
            .ToListAsync();
    }
}