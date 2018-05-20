﻿namespace Falcon.API.Models
{
    using System.Collections.Generic;
    using Falcon.API.Helpers;
    using Falcon.MtG;
    using Newtonsoft.Json;

    public class GeneratorRequestModel
    {
        private const string ArtifactsKey = "artifacts";

        private const string AurasKey = "auras";

        private const string BasicLandsKey = "basicLands";

        private const string CreaturesKey = "creatures";

        private const string EnchantmentsKey = "enchantments";

        private const string EquipmentKey = "equipment";

        private const string LegendaryKey = "legendary";

        private const string ManaProducingKey = "manaProducing";

        private const string NonbasicLandsKey = "nonbasicLands";

        private const string PlaneswalkersKey = "planeswalkers";

        private const string SharesTypesKey = "sharesTypes";

        private const string SpellsKey = "spells";

        public GeneratorRequestModel()
        {
            this.Categories = new List<Category>();
            this.CategoryIndex = new Dictionary<string, Category>();
            this.SetCodes = new List<string>();
        }

        public Category Artifacts
        {
            get
            {
                return this[ArtifactsKey];
            }

            set
            {
                this[ArtifactsKey] = value;
            }
        }

        public Category Auras
        {
            get
            {
                return this[AurasKey];
            }

            set
            {
                this[AurasKey] = value;
            }
        }

        public Category BasicLands
        {
            get
            {
                return this[BasicLandsKey];
            }

            set
            {
                this[BasicLandsKey] = value;
            }
        }

        [JsonProperty("categories")]
        public List<Category> Categories { get; set; }

        [JsonProperty("cmc")]
        public Range CMC { get; set; }

        [JsonProperty("cmdr1Id")]
        public int? Commander1Id { get; set; }

        [JsonProperty("cmdr2Id")]
        public int? Commander2Id { get; set; }

        [JsonProperty("format")]
        public EdhFormat Format { get; set; }

        public Category Creatures
        {
            get
            {
                return this[CreaturesKey];
            }

            set
            {
                this[CreaturesKey] = value;
            }
        }

        public Category Enchantments
        {
            get
            {
                return this[EnchantmentsKey];
            }

            set
            {
                this[EnchantmentsKey] = value;
            }
        }

        public Category Equipment
        {
            get
            {
                return this[EquipmentKey];
            }

            set
            {
                this[EquipmentKey] = value;
            }
        }

        public Category Legendary
        {
            get
            {
                return this[LegendaryKey];
            }

            set
            {
                this[LegendaryKey] = value;
            }
        }

        public Category ManaProducing
        {
            get
            {
                return this[ManaProducingKey];
            }

            set
            {
                this[ManaProducingKey] = value;
            }
        }

        public Category NonbasicLands
        {
            get
            {
                return this[NonbasicLandsKey];
            }

            set
            {
                this[NonbasicLandsKey] = value;
            }
        }

        public Category Planeswalkers
        {
            get
            {
                return this[PlaneswalkersKey];
            }

            set
            {
                this[PlaneswalkersKey] = value;
            }
        }

        [JsonProperty("sets")]
        public List<string> SetCodes { get; set; }

        public Category SharesTypes
        {
            get
            {
                return this[SharesTypesKey];
            }

            set
            {
                this[SharesTypesKey] = value;
            }
        }

        public Category Spells
        {
            get
            {
                return this[SpellsKey];
            }

            set
            {
                this[SpellsKey] = value;
            }
        }

        private Dictionary<string, Category> CategoryIndex { get; set; }

        public Category this[string name]
        {
            get
            {
                if (!this.CategoryIndex.ContainsKey(name))
                {
                    this.CategoryIndex.Add(name, this.Categories.Find(c => c.Name == name));
                }

                return this.CategoryIndex[name];
            }

            set
            {
                if (!this.CategoryIndex.ContainsKey(name))
                {
                    this.CategoryIndex.Add(name, this.Categories.Find(c => c.Name == name));
                }

                this.CategoryIndex[name] = value;
            }
        }
    }
}