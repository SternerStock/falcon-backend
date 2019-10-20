namespace Falcon.MtG.DBSync
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public static class FileUtility
    {
        public static async Task<string> ReadAllTextAsync(string filePath)
        {
            var sb = new StringBuilder();
            using (var stream = File.OpenRead(filePath))
            {
                using (var reader = new StreamReader(stream))
                {
                    string line = await reader.ReadLineAsync();
                    while (line != null)
                    {
                        sb.AppendLine(line);
                        line = await reader.ReadLineAsync();
                    }

                    return sb.ToString();
                }
            }
        }
    }
}