﻿namespace Falcon.MtG.Models.Sql
{
    public class CardColor
    {
        public int CardID { get; set; }
        public Card Card { get; set; }
        public int ColorID { get; set; }
        public Color Color { get; set; }
    }
}