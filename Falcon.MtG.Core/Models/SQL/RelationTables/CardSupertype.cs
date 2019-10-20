namespace Falcon.MtG
{
    public class CardSupertype
    {
        public int CardID { get; set; }
        public Card Card { get; set; }
        public int SupertypeID { get; set; }
        public Supertype Supertype { get; set; }
    }
}