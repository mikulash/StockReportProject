using HtmlAgilityPack;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Scraper;

public class Scraper
{
    private readonly ILogger _logger;
    private const string url = "https://ark-funds.com";

    public Scraper(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Scraper>();
    }

    [Function("Scraper")]
    // public void Run([TimerTrigger("0 0 2 * * *")] TimerInfo myTimer)
    public void Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {Now}", DateTime.Now);
        Scrape().Wait();
    }

    private static async Task Scrape()
    {
        string fileUrl = await ExtractFileUrl(url) ?? string.Empty;
        if (!string.IsNullOrEmpty(fileUrl))
        {
            await DownloadFileAsync(fileUrl, "DownloadedFile.csv");
            Console.WriteLine("File downloaded successfully.");
        }
        else
        {
            Console.WriteLine("File URL not found.");
        }
    }
    private static async Task<string?> ExtractFileUrl(string pageUrl)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(pageUrl);

        var link = doc.DocumentNode.SelectSingleNode("//a[@class='b-documents-item-link']");
        return link?.Attributes["href"]?.Value;
    }

    private static async Task DownloadFileAsync(string fileUrl, string savePath)
    {
        using HttpClient client = new HttpClient();
        using HttpResponseMessage response = await client.GetAsync(fileUrl);
        await using Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();
        await using Stream streamToWriteTo = File.Open(savePath, FileMode.Create);
        await streamToReadFrom.CopyToAsync(streamToWriteTo);
    }
}
