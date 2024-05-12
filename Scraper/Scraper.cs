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

    public Scraper(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Scraper>();
    }

    [Function("Scraper")]
    // public void Run([TimerTrigger("0 0 2 * * *")] TimerInfo myTimer)
    public void Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer) // every minute for development
    {
        _logger.LogInformation("C# Timer trigger function executed at: {Now}", DateTime.Now);
        Scrape().Wait();
    }

    private async Task Scrape()
    {
        IWebDriver? driver;
        try
        {
            var chromeOptions = new ChromeOptions();

            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");
            driver = new ChromeDriver(chromeOptions);
            driver.Navigate().GoToUrl(url);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));  // Adjust timeout as necessary


            //agree cookies
            driver.FindElement(By.XPath("//*[@id=\"agree_button\"]")).Click();
            _logger.LogInformation("Agreed to cookies");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            driver.FindElement(By.XPath("//*[@id=\"hs-eu-confirmation-button\"]")).Click();
            _logger.LogInformation("Agreed to cookies 2");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            // select document type
            IWebElement element = driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div[2]/div/div/div/div/div[2]/div[1]/div/ul/li[8]/a"));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            _logger.LogInformation("Scrolled to element");
            Thread.Sleep(1000); // Consider using a more reliable waiting method in production
            element.Click();
            _logger.LogInformation("Clicked on element");
            // driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div[2]/div/div/div/div/div[2]/div[1]/div/ul/li[8]/a")).Click();
            // select document
            var downloadLink = driver.FindElement(By.XPath("//*[@id=\"doc-1793\"]/a")).GetAttribute("href");
            driver.Close();
            await DownloadFileAsync(downloadLink);
            Console.WriteLine("File downloaded successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }

    private async Task DownloadFileAsync(string fileUrl)
    {
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
        using HttpResponseMessage response = await client.GetAsync(fileUrl);
        if (response.IsSuccessStatusCode)
        {
            await using Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();
            string datedFilename = $"{filename}_{DateTime.Now:dd_MM_yyyy}";
            _logger.LogInformation("Downloading file to: {DatedFilename}", datedFilename);

            // Define the path locally if needed for debugging or additional processing
            string savePath = Path.Combine(Environment.CurrentDirectory, datedFilename);
            await using Stream streamToWriteTo = File.Open(savePath, FileMode.Create);
            await streamToReadFrom.CopyToAsync(streamToWriteTo);
            // Create the blob in Azure Blob Storage and upload the file
            await UploadToBlobAsync(streamToReadFrom, datedFilename);
        }
        else
        {
            _logger.LogError("Failed to download file. Status Code: {StatusCode}", response.StatusCode);
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
