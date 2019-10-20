namespace Falcon.MtG
{
    public class Legality
    {
        public int ID { get; set; }

        public string Format { get; set; }

        public bool LegalAsCommander { get; set; }

        public bool Legal { get; set; }

        public int CardID { get; set; }

        public Card Card { get; set; }
    }
}