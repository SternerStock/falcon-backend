namespace Falcon.MtG.Models.Sql
{
    using System;

    public class Pricing
    {
        public int ID { get; set; }

        public int PrintingID { get; set; }

        public DateTime Date { get; set; }

        public double Price { get; set; }

        public bool Foil { get; set; }

        public Printing Printing { get; set; }
    }
}