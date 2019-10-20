namespace Falcon.MtG
{
    public class CardKeyword
    {
        public int CardID { get; set; }
        public Card Card { get; set; }
        public int KeywordID { get; set; }
        public Keyword Keyword { get; set; }
    }
}