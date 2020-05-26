namespace Falcon.MtG.Models.Sql
{
    public class AlsoKnownAs : ISimpleLookup
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }
}