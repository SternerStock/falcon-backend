namespace Falcon.XorYDatabase.Models.Json
{
    public class QuestionRequest
    {
        public string[] Categories { get; set; } = [];
        public int[] SeenOptions { get; set; } = [];
    }
}
