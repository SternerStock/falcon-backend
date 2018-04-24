namespace Falcon.API.Models
{
    using Falcon.MtG;
    using Newtonsoft.Json;

    public class ResponseSet
    {
        public ResponseSet(Set s)
        {
            this.Id = s.ID;
            this.Name = s.Name;
            this.Code = s.Code;
        }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}