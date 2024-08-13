using Falcon.MtG.Models;

namespace Falcon.API.DTO
{
    public class DeckResponseDto(Deck deck)
    {
        public int Seed { get; set; } = deck.Seed;
        public int BasicLands { get; set; } = deck.BasicLands;
        public int NonbasicLands { get; set; } = deck.NonbasicLands;
        public int Creatures { get; set; } = deck.Creatures;
        public int Artifacts { get; set; } = deck.Artifacts;
        public int Equipment { get; set; } = deck.Equipment;
        public int Vehicles { get; set; } = deck.Vehicles;
        public int Enchantments { get; set; } = deck.Enchantments;
        public int Auras { get; set; } = deck.Auras;
        public int Planeswalkers { get; set; } = deck.Planeswalkers;
        public int Spells { get; set; } = deck.Spells;
        public int ManaProducing { get; set; } = deck.ManaProducing;
        public int Legendary { get; set; } = deck.Legendary;
        public string Issues { get; set; } = deck.Issues.ToString();
        public string Cards { get; set; } = deck.ToString();
    }
}