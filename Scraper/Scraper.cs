using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Azure.Storage.Blobs;
using Azure.Identity;
namespace Scraper;

public class Scraper
{
    private readonly ILogger _logger;
    private const string url = "https://ark-funds.com/download-fund-materials/";
    private const string filename = "ARK_FUNDS";
    private const string localDbEndpoint = "http://localhost:5177/api/indexrecord/upload";
    private const string fallbackUrl = "https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";

    public Scraper(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Scraper>();
    }

    [Function("Scraper")]
    public void Run([TimerTrigger("0 0 2 * * *")] TimerInfo myTimer) // every day at 2am
    // public void Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer) // every minute for development
    {
        _logger.LogInformation("C# Timer trigger function executed at: {Now}", DateTime.Now);
        Scrape().Wait();
    }

    private async Task Scrape()
    {
        var downloadLink = "";
        try
        {
            var driver = CreateChromeDriver();
            downloadLink = ScrapeDownloadLink(driver);
            driver.Close();
        }
        catch (Exception ex)
        {
            downloadLink = fallbackUrl;
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {

            await StoreFileAsync(downloadLink);
            Console.WriteLine("File downloaded successfully!");
        }
    }

    private static ChromeDriver CreateChromeDriver()
    {
        var chromeOptions = new ChromeOptions();

        chromeOptions.AddArgument("--headless");
        chromeOptions.AddArgument("--no-sandbox");
        chromeOptions.AddArgument("--disable-dev-shm-usage");
        chromeOptions.AddArgument("--disable-gpu");
        chromeOptions.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");
        return new ChromeDriver(chromeOptions);
    }

    private string ScrapeDownloadLink(ChromeDriver driver)
    {
        driver.Navigate().GoToUrl(url);
        //agree cookies
        driver.FindElement(By.XPath("//*[@id=\"agree_button\"]")).Click();
        driver.FindElement(By.XPath("//*[@id=\"hs-eu-confirmation-button\"]")).Click();
        // select document type
        IWebElement element = driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div[2]/div/div/div/div/div[2]/div[1]/div/ul/li[8]/a"));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        _logger.LogInformation("Scrolled to element");
        Thread.Sleep(1000); // Consider using a more reliable waiting method in production
        element.Click();
        _logger.LogInformation("Clicked on element");
        // select document
        var downloadLink = driver.FindElement(By.XPath("//*[@id=\"doc-1793\"]/a")).GetAttribute("href");
        return downloadLink;
    }

    private async Task StoreFileAsync(string fileUrl)
    {
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
        using HttpResponseMessage response = await client.GetAsync(fileUrl);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to download file. Status Code: {StatusCode}", response.StatusCode);
            throw new InvalidOperationException("Failed to download file");
        }
        await using Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();

        string datedFilename = $"{filename}_{DateTime.Now:dd_MM_yyyy}";
        // Store the file locally
        await StoreFileLocally(streamToReadFrom, datedFilename);
        // Upload the file to the local database
        await UploadToLocalDbAsync(streamToReadFrom, datedFilename);
        // Create the blob in Azure Blob Storage and upload the file
        await UploadToBlobAsync(streamToReadFrom, datedFilename);
    }

    private async Task StoreFileLocally(Stream dataStream, string datedFilename)
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
        _logger.LogInformation("Downloading file to: {DatedFilename}", datedFilename);
        string savePath = Path.Combine(Environment.CurrentDirectory, datedFilename);
        await using Stream streamToWriteTo = File.Open(savePath, FileMode.Create);
        await dataStream.CopyToAsync(streamToWriteTo);
    }

    private async Task UploadToLocalDbAsync(Stream fileStream, string datedFilename)
    {
        using HttpClient client = new HttpClient();
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream)
        {
            Headers = { ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = datedFilename
            }}
        }, "file", datedFilename);

        using HttpResponseMessage response = await client.PostAsync(localDbEndpoint, content);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to upload file to endpoint. Status Code: {StatusCode}", response.StatusCode);
            throw new InvalidOperationException("Failed to upload file to endpoint");
        }
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
