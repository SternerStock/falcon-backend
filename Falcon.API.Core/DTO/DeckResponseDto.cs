using Falcon.MtG.Models;

namespace Falcon.API.DTO
{
    public class DeckResponseDto
    {
        public int Seed { get; set; }
        public int BasicLands { get; set; }
        public int NonbasicLands { get; set; }
        public int Creatures { get; set; }
        public int Artifacts { get; set; }
        public int Equipment { get; set; }
        public int Vehicles { get; set; }
        public int Enchantments { get; set; }
        public int Auras { get; set; }
        public int Planeswalkers { get; set; }
        public int Spells { get; set; }
        public int ManaProducing { get; set; }
        public int Legendary { get; set; }
        public string Issues { get; set; }
        public string Cards { get; set; }

        public DeckResponseDto(Deck deck)
        {
            Seed = deck.Seed;
            BasicLands = deck.BasicLands;
            NonbasicLands = deck.NonbasicLands;
            Creatures = deck.Creatures;
            Artifacts = deck.Artifacts;
            Equipment = deck.Equipment;
            Vehicles = deck.Vehicles;
            Enchantments = deck.Enchantments;
            Auras = deck.Auras;
            Planeswalkers = deck.Planeswalkers;
            Spells = deck.Spells;
            ManaProducing = deck.ManaProducing;
            Legendary = deck.Legendary;
            Issues = deck.Issues.ToString();
            Cards = deck.ToString();
        }
    }
}