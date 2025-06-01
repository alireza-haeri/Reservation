string endpoint = "https://localhost:7073";

Console.WriteLine("Please write 1 ro 2, 1: redis locker - 2: db locker");
var lockerType = int.Parse(Console.ReadLine()!);
endpoint += lockerType switch
{
    1 => "/counter-red-lock",
    2 => "/counter-db",
    _ => throw new ArgumentOutOfRangeException()
};

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