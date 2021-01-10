namespace Falcon.MtG.Models.Json
{
    public class JsonMeta
    {
        public JsonMeta ()
        {
            this.Data = new JsonVersion();
            this.Meta = new JsonVersion();
        }

        public JsonVersion Data { get; set; }

        public JsonVersion Meta { get; set; }
    }
}