namespace Falcon.MtG.Models.Sql
{
    public interface ISimpleLookup
    {
        int ID { get; set; }
        string Name { get; set; }
    }
}