namespace Falcon.MtG
{
    public class CardTypeSupertype
    {
        public int CardTypeID { get; set; }
        public CardType CardType { get; set; }
        public int SupertypeID { get; set; }
        public Supertype Supertype { get; set; }
    }
}