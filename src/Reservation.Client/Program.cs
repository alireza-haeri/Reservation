const string endpoint = "https://localhost:7073";

Console.WriteLine("please write a number of request!");
int requestCount = int.Parse(Console.ReadLine()!);

var client = new HttpClient();
client.BaseAddress = new Uri(endpoint);

var responses = Enumerable.Range(0, requestCount)
    .Select(async (e) => await SendRequestAsync(client, e.ToString()))
    .ToList();

await Task.WhenAll(responses);

foreach (var response in responses)
    Console.WriteLine($"Request: {response.Id}: Result: {response.Result.StatusCode}");

Console.WriteLine("Completed ...");
Console.ReadKey();

return;

static async Task<HttpResponseMessage> SendRequestAsync(HttpClient client, string requestId)
{
    Console.WriteLine($"Request: {requestId}: Sending request");
    return await client.GetAsync("");
}