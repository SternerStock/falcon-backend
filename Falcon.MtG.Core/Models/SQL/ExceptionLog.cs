namespace Falcon.MtG
{
    public class ExceptionLog
    {
        public int ID { get; set; }

        public string RequestJson { get; set; }

        public string Outcome { get; set; }

        public string Exception { get; set; }
    }
}