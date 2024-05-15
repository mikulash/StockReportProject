using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Identity;
namespace Scraper;

public class Scraper
{
    private readonly ILogger _logger;
    private const string filename = "ARK_FUNDS";

    public Scraper(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Scraper>();
    }

    [Function("Scraper")]
    // public void Run([TimerTrigger("0 0 2 * * *")] TimerInfo myTimer) // every day at 2am
    public void Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer) // every minute for development
    {
        var scraperCore = new ScraperCore.ScraperCore();
        var downloadLink = scraperCore.ScrapeDownloadLink();
        Stream fileStream = scraperCore.DownloadFileAsync(downloadLink).Result;
        string datedFilename = $"{filename}_{DateTime.Now:dd_MM_yyyy}";
        _ = UploadToBlobAsync(fileStream, datedFilename);
    }

    private async Task UploadToBlobAsync(Stream dataStream, string blobName)
    {
        if (dataStream.CanSeek)
        {
            dataStream.Position = 0;
        }
        else
        {
            _logger.LogError("Stream cannot seek to the beginning");
            throw new InvalidOperationException("Stream must be seekable.");
        }

        string containerEndpoint = "https://stockreport24.blob.core.windows.net/funds";

        BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndpoint),
            new DefaultAzureCredential());

        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        _logger.LogInformation("Uploading to Blob Storage as {BlobName}", blobName);
        await blobClient.UploadAsync(dataStream, overwrite: true);
        _logger.LogInformation("Upload complete");
    }
}
