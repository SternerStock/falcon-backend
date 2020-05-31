namespace Falcon.MtG.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Falcon.MtG.Models.Sql;
    using Falcon.MtG.Utility;

    public class Deck
    {
        public int Seed { get; set; }
        public bool Singleton { get; set; }
        public List<Card> Cards { get; set; }
        public string Issues { get; set; }

        public Deck()
        {
            Cards = new List<Card>();
        }

        public int BasicLands
        {
            get
            {
                return Cards.AsQueryable().BasicLandFilter().Count();
            }
        }

        public int NonbasicLands
        {
            get
            {
                return Cards.AsQueryable().NonbasicLandFilter().Count();
            }
        }

        public int Creatures
        {
            get
            {
                return Cards.AsQueryable().CreatureFilter().Count();
            }
        }

        public int Artifacts
        {
            get
            {
                return Cards.AsQueryable().ArtifactFilter().Count();
            }
        }

        public int Equipment
        {
            get
            {
                return Cards.AsQueryable().EquipmentFilter().Count();
            }
        }

        public int Vehicles
        {
            get
            {
                return Cards.AsQueryable().VehicleFilter().Count();
            }
        }

        public int Enchantments
        {
            get
            {
                return Cards.AsQueryable().EnchantmentFilter().Count();
            }
        }

        public int Auras
        {
            get
            {
                return Cards.AsQueryable().AuraFilter().Count();
            }
        }

        public int Planeswalkers
        {
            get
            {
                return Cards.AsQueryable().PlaneswalkerFilter().Count();
            }
        }

        public int Spells
        {
            get
            {
                return Cards.AsQueryable().SpellFilter().Count();
            }
        }

        public int ManaProducing
        {
            get
            {
                return Cards.AsQueryable().ManaProducingFilter().Count();
            }
        }

        public int Legendary
        {
            get
            {
                return Cards.AsQueryable().LegendaryFilter().Count();
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            var groups = this.Cards.GroupBy(c => c.CockatriceName);
            foreach (var group in groups)
            {
                stringBuilder.AppendLine($"{group.Count()} {group.Key}");
            }

            return stringBuilder.ToString();
        }
    }
}