using System.Net.Http;
using System.Text.Json;
using System.Web;
using System.IO;
using reader;
using classsetup;
using parser;

// var results = new List<object>();

// var client = new HttpClient();
// client.DefaultRequestHeaders.Add("User-Agent", "statreader_terraria (fabian)");

//// USING QUERY

// string pageTitle = "Eye_of_Cthulhu";

// var query = HttpUtility.ParseQueryString(string.Empty);
// query["action"] = "query";
// query["titles"] = pageTitle;
// query["prop"] = "revisions"; // I want the page revisions (edits)
// query["rvprop"] = "content";
// query["rvslots"] = "main";      // I want main content
// query["format"] = "json";
// query["formatversion"] = "2";   // cleaner JSON shape, new since 2025

// var url = $"https://terraria.wiki.gg/api.php?{query}";

// try
// {
//     var response = await client.GetStringAsync(url);

//     using var doc = JsonDocument.Parse(response);
//     var pages = doc.RootElement
//         .GetProperty("query")
//         .GetProperty("pages");

//         // JSON format comes like this:
//         // {
//         //   "query": {
//         //     "pages": [
//         //       {
//         //         "pageid": 12345, ...

//     foreach (var page in pages.EnumerateArray())
//     {
//         if (page.TryGetProperty("missing", out _))
//         {
//             Console.WriteLine($"Page '{pageTitle}' not found.");
//             continue;
//         }

//         var title = page.GetProperty("title").GetString();
//         var wikitext = page
//             .GetProperty("revisions")[0]
//             .GetProperty("slots")
//             .GetProperty("main")
//             .GetProperty("content")
//             .GetString();

//         results.Add(new { title, wikitext });
//     }

//     var outputJson = JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
//     File.WriteAllText("output.json", outputJson);
// }
// catch (HttpRequestException ex)
// {
//     Console.WriteLine($"Request failed: {ex.Message}");
// }

//// USING CARGOQUERY

// var query = HttpUtility.ParseQueryString(string.Empty);
// query["action"] = "cargoquery";
// query["tables"] = "Drops";
// query["fields"] = "_pageName,item,quantity,rate,normal,expert,master";
// query["where"] = "_pageName=\"Eye of Cthulhu\" AND isfromnpc=true";
// query["format"] = "json";
// query["formatversion"] = "2";

// var url = $"https://terraria.wiki.gg/api.php?{query}";
// var response = await client.GetStringAsync(url);

// using var doc = JsonDocument.Parse(response);
// var rows = doc.RootElement.GetProperty("cargoquery");


// foreach (var row in rows.EnumerateArray())
// {
//     var title = row.GetProperty("title"); // Cargo nests each row under "title"

//     string item = title.GetProperty("item").GetString();
//     string quantity = title.GetProperty("quantity").GetString();
//     string rate = title.GetProperty("rate").GetString();

//     Console.WriteLine($"{item} x{quantity} — {rate}");
// }

// JSON_Experiment.Run();

// await ClassSetup.Run();



Parser.Run();