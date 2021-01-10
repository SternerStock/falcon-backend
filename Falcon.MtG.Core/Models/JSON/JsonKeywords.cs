namespace Falcon.MtG.Models.Json
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class JsonKeywords
    {
        public List<string> AbilityWords { get; set; }

        public List<string> KeywordAbilities { get; set; }

        public List<string> KeywordActions { get; set; }
    }
}