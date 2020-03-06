namespace Falcon.MtG.Models.Sql
{
    using System.Collections.Generic;

    public class SetType : ISimpleLookup
    {
        public SetType()
        {
            this.Sets = new HashSet<Set>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<Set> Sets { get; set; }
    }
}