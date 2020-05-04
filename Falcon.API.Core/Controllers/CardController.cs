namespace Falcon.API.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Falcon.API.DTO;
    using Falcon.MtG;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly MtGDBContext context;

        public CardController(MtGDBContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public async Task<CardDto> Get(int id) => await this.context.Cards
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
            .Where(l => l.Format == variant && l.LegalAsCommander)
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

        [HttpPost("SignatureSpells")]
        public async Task<List<CardDto>> GetSignatureSpells(string[] ColorIdentity) => await this.context.Legalities
            .Where(l => l.Format == "Oathbreaker" && l.Legal && !ColorIdentity.Except(l.Card.ColorIdentity.Select(ci => ci.Color.Name)).Any())
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

        [HttpGet("RandomFlavor")]
        public async Task<dynamic> GetRandomFlavorText() => await this.context.Printings
            .Where(p => !string.IsNullOrEmpty(p.FlavorText))
            .OrderBy(p => Guid.NewGuid())
            .Select(p => new
            {
                p.FlavorText,
                p.Card.Name
            })
            .FirstAsync();
    }
}