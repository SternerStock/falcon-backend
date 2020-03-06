namespace Falcon.MtG.Models.Sql
{
    using System;
    using System.Collections.Generic;

    public class Set
    {
        public Set()
        {
            this.Printings = new HashSet<Printing>();
        }

        public int ID { get; set; }

        public string Code { get; set; }

        public string KeyruneCode { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public int? SetTypeID { get; set; }

        public int? BlockID { get; set; }

        public ICollection<Printing> Printings { get; set; }

        public SetType SetType { get; set; }

        public Block Block { get; set; }
    }
}