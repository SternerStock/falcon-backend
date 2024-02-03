namespace Falcon.MtG.Models.Json
{
    using System.Collections.Generic;

    public class JsonCardTypesWrapper
    {
        public dynamic Types { get; set; }
    }

    public class JsonCardTypes
    {
        public List<string> SubTypes { get; set; }

        public List<string> SuperTypes { get; set; }
    }
}