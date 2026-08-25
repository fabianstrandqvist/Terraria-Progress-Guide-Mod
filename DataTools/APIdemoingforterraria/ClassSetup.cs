using System.Net.Http;
using System.Text.Json;
using System.Web;
using System.IO;
using reader;

namespace classsetup
{
    public class ClassSetup
    {
        public static async Task Run()
        {
            string status = await Query();
            Console.WriteLine(status);
        }

        private static async Task<string> Query()
        {
            var results = new List<object>();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "statreader_terraria (fabian)");

            //// USING QUERY
            string pageTitle = "Guide:Class setups"; // unsure

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "query";
            query["titles"] = pageTitle;
            query["prop"] = "revisions"; // I want the page revisions (edits)
            query["rvprop"] = "content";
            query["rvslots"] = "main";      // I want main content
            query["format"] = "json";
            query["formatversion"] = "2";   // cleaner JSON shape, new since 2025

            var url = $"https://terraria.wiki.gg/api.php?{query}";

            try
            {
                var response = await client.GetStringAsync(url);

                using var doc = JsonDocument.Parse(response);
                var pages = doc.RootElement
                    .GetProperty("query")
                    .GetProperty("pages");

                    // JSON format comes like this:
                    // {
                    //   "query": {
                    //     "pages": [
                    //       {
                    //         "pageid": 12345, ...

                foreach (var page in pages.EnumerateArray())
                {
                    if (page.TryGetProperty("missing", out _))
                    {
                        Console.WriteLine($"Page '{pageTitle}' not found.");
                        continue;
                    }

                    var title = page.GetProperty("title").GetString();
                    var wikitext = page
                        .GetProperty("revisions")[0]
                        .GetProperty("slots")
                        .GetProperty("main")
                        .GetProperty("content")
                        .GetString();

                    results.Add(new { title, wikitext });
                }

                var outputJson = JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("output.json", outputJson);
                return "Query completed successfully.";
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request failed: {ex.Message}");
                return "Query failed.";
            }
        }
    }
}
