namespace Falcon.API.DTO
{
    public class GenerateDeckDto
    {
        public int? Seed { get; set; }
        public string DeckType { get; set; }
        public string Format { get; set; }
        public int DeckSize { get; set; }
        public bool SilverBorder { get; set; }
        public int? CommanderId { get; set; }
        public int? PartnerId { get; set; }
        public int? SignatureSpellId { get; set; }
        public string[] ColorIdentity { get; set; }
        public SimpleRange EdhRecRange { get; set; }
        public SimpleRange CmcRange { get; set; }
        public int[] SetIds { get; set; }
        public int[] RarityIds { get; set; }
        public int[] ArtistIds { get; set; }
        public int[] FrameIds { get; set; }
        public int BasicLands { get; set; }
        public int NonbasicLands { get; set; }
        public int Creatures { get; set; }
        public int SharesType { get; set; }
        public int Artifacts { get; set; }
        public int Equipment { get; set; }
        public int Vehicles { get; set; }
        public int Enchantments { get; set; }
        public int Auras { get; set; }
        public int Planeswalkers { get; set; }
        public int Spells { get; set; }
        public int ManaProducing { get; set; }
        public int Legendary { get; set; }
    }
}