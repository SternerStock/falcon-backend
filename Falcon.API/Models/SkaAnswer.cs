namespace Falcon.API.Models
{
    public enum SkaAnswerType
    {
        ska,
        kid
    }

    public class SkaAnswer
    {
        public string Name { get; set; }
        public SkaAnswerType Type { get; set; }
        public string URL { get; set; }
    }
}