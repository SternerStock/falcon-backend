namespace Falcon.MtG.Models.Json
{
    public class JsonMeta<T> where T : new()
    {
        public JsonMeta ()
        {
            this.Data = new T();
            this.Meta = new JsonVersion();
        }

        public T Data { get; set; }

        public JsonVersion Meta { get; set; }
    }
}