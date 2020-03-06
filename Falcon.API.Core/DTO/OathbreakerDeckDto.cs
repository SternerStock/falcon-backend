namespace Falcon.API.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OathbreakerDeckDto : DeckDto
    {
        public string Oathbreaker { get; set; }

        public string SignatureSpell { get; set; }
    }
}
