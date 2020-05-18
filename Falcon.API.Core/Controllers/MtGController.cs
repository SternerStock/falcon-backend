namespace Falcon.API.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Falcon.API.DTO;
    using Falcon.MtG;
    using Falcon.MtG.Models.Sql;
    using Falcon.MtG.Utility;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class MtGController : ControllerBase
    {
        private readonly string[] SetTypes;
        private readonly MtGDBContext context;

        public MtGController(MtGDBContext context)
        {
            this.context = context;
            this.SetTypes = new string[] { "core", "expansion", "masters", "planechase", "archenemy", "commander" };
        }

        [HttpGet("Card/{id}")]
        public async Task<CardDto> GetCard(int id) => await this.context.Cards
            .Where(c => c.ID == id)
            .IncludeCardProperties()
            .Select(c => new CardDto(c))
            .SingleOrDefaultAsync();

        [HttpGet("Commanders")]
        public async Task<IEnumerable<CardDto>> GetCommanders(string variant = "Commander", bool allowSilver = false) => await this.context.Legalities
            .Where(l => l.Format == variant.Replace(" ", string.Empty) && l.LegalAsCommander && (l.Legal || (allowSilver && l.Card.Printings.All(p => p.Border.Name == "silver"))))
            .IncludeCardProperties()
            .OrderBy(l => l.Card.Name)
            .Select(l => new CardDto(l.Card))
            .ToListAsync();

        [HttpGet("Partners")]
        public async Task<IEnumerable<CardDto>> GetPartners(int cmdrId, string variant = "Commander", bool allowSilver = false)
        {
            var cmdr = await this.context.Cards.SingleAsync(c => c.ID == cmdrId);
            if (!cmdr.OracleText.Contains("Partner"))
            {
                return new List<CardDto>();
            }

            var legalities = this.context.Legalities
                .Where(l => l.Format == variant.Replace(" ", string.Empty) && l.LegalAsCommander && (l.Legal || (allowSilver && l.Card.Printings.All(p => p.Border.Name == "silver"))) && l.CardID != cmdrId);

            if (cmdr.OracleText.Contains("Partner with"))
            {
                var match = Regex.Match(cmdr.OracleText, @"Partner with ([\w, ]*)( \(|$)");
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

        [HttpGet("SignatureSpells")]
        public async Task<IEnumerable<CardDto>> GetSignatureSpells(int obId, bool allowSilver = false)
        {
            var colorIdentity = this.context.CardColorIdentities.Where(c => c.CardID == obId)
                .Include(c => c.Color).Select(c => c.Color.Symbol);

            return await this.context.Legalities
            .Where(l =>
                l.Format == "Oathbreaker" && (l.Legal || (allowSilver && l.Card.Printings.All(p => p.Border.Name == "silver")))
                && l.Card.Types.Any(t => t.CardType.Name == "instant" || t.CardType.Name == "sorcery")
                && !l.Card.ColorIdentity.Select(ci => ci.Color.Symbol).Except(colorIdentity).Any())
            .IncludeCardProperties()
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
        public async Task<IEnumerable<SetDto>> GetSets(string variant = "Commander", bool allowSilver = false)
        {
            if (variant == "Penny Dreadful")
            {
                variant = "Penny";
            }

            var listOfSets = await this.context.Legalities
            .Where(l => l.Format == variant.Replace(" ", string.Empty) && (l.Legal || (allowSilver && l.Card.Printings.All(p => p.Border.Name == "silver"))) && !l.Card.Supertypes.Any(t => t.Supertype.Name == "Basic"))
            .Where(l => l.Card.Printings.Any(p => this.SetTypes.Contains(p.Set.Name) || (allowSilver && p.Set.SetType.Name == "funny")))
            .Select(l => l.Card.Printings.Select(p => p.Set))
            .ToListAsync();

            List<Set> sets = new List<Set>();
            foreach (var list in listOfSets)
            {
                foreach(var set in list)
                {
                    if (!sets.Any(s => s.ID == set.ID))
                    {
                        sets.Add(set);
                    }
                }
            }

            return sets
            .OrderByDescending(s => s.Date)
            .Select(s => new SetDto(s));
        }

        [HttpGet("Watermarks")]
        public async Task<IEnumerable<KeyValueDto>> GetWatermarks() => await this.context.Watermarks
            .OrderBy(w => w.Name)
            .Select(w => new KeyValueDto
            {
                ID = w.ID,
                Name = w.Name
            })
            .ToListAsync();

        [HttpGet("Rarities")]
        public async Task<IEnumerable<KeyValueDto>> GetRarities() => await this.context.Rarities
            .OrderBy(r => r.Name == "common" ? 1 : r.Name == "uncommon" ? 2 : r.Name == "rare" ? 3 : r.Name == "mythic" ? 4 : 5)
            .Select(r => new KeyValueDto
            {
                ID = r.ID,
                Name = r.Name
            })
            .ToListAsync();

        [HttpGet("Frames")]
        public async Task<IEnumerable<KeyValueDto>> GetFrames() => await this.context.Frames
            .OrderBy(f => f.Name)
            .Select(f => new KeyValueDto
            {
                ID = f.ID,
                Name = f.Name
            })
            .ToListAsync();

        [HttpGet("Layouts")]
        public async Task<IEnumerable<KeyValueDto>> GetLayouts() => await this.context.Layouts
            .OrderBy(l => l.Name)
            .Select(l => new KeyValueDto
            {
                ID = l.ID,
                Name = l.Name
            })
            .ToListAsync();

        [HttpGet("Artists")]
        public async Task<IEnumerable<KeyValueDto>> GetArtists() => await this.context.Artists
            .OrderBy(a => a.Name)
            .Select(a => new KeyValueDto
            {
                ID = a.ID,
                Name = a.Name
            })
            .ToListAsync();
    }
}