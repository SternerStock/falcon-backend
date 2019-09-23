namespace Falcon.MtG.MtgJsonModels
{
    using System;

    public class JsonVersion
    {
        public JsonVersion()
        {
            this.Date = DateTime.MinValue;
            this.Version = "Unknown";
        }

        public DateTime Date { get; set; }

        public string Version { get; set; }
    }
}