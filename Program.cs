using Newtonsoft.Json;

namespace SentinelTestAdo
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = JsonConvert.SerializeObject(new { message = "Test app for Azure DevOps integration" });
            Console.WriteLine(json);
        }
    }
}
