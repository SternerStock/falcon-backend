namespace Falcon.API.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Falcon.API.DTO;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class DeckController : ControllerBase
    {
        [HttpPost]
        [ActionName("Generate")]
        public async Task<DeckDto> Generate([FromBody]GenerateDeckDto deckSettings)
        {
            throw new NotImplementedException();
        }
    }
}