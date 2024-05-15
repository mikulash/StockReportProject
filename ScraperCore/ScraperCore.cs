using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ScraperCore;

public class ScraperCore
{
    private const string url = "https://ark-funds.com/download-fund-materials/";
    private const string filename = "ARK_FUNDS";
    private const string localDbEndpoint = "http://localhost:5177/api/indexrecord/upload";

    private const string fallbackUrl =
        "https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";

    public Task Scrape()
    {
        var downloadLink = ScrapeDownloadLink();
        StoreFileAsync(downloadLink).Wait();
        Console.WriteLine("Scraping complete");
        return Task.CompletedTask;
    }

    private static ChromeDriver CreateChromeDriver()
    {
        var chromeOptions = new ChromeOptions();

        chromeOptions.AddArgument("--headless");
        chromeOptions.AddArgument("--no-sandbox");
        chromeOptions.AddArgument("--disable-dev-shm-usage");
        chromeOptions.AddArgument("--disable-gpu");
        chromeOptions.AddArgument(
            "--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");
        return new ChromeDriver(chromeOptions);
    }

    public string ScrapeDownloadLink()
    {
        string downloadLink;
        try
        {
            ChromeDriver driver = CreateChromeDriver();
            driver.Navigate().GoToUrl(url);
            //agree cookies
            driver.FindElement(By.XPath("//*[@id=\"agree_button\"]")).Click();
            driver.FindElement(By.XPath("//*[@id=\"hs-eu-confirmation-button\"]")).Click();
            // select document type
            IWebElement element =
                driver.FindElement(
                    By.XPath("/html/body/div[5]/div[2]/div[2]/div/div/div/div/div[2]/div[1]/div/ul/li[8]/a"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            Thread.Sleep(1000); // Consider using a more reliable waiting method in production
            element.Click();
            // select document
            downloadLink = driver.FindElement(By.XPath("//*[@id=\"doc-1793\"]/a")).GetAttribute("href");
            Console.WriteLine("Scraped download link: " + downloadLink);
            driver.Close();
        }
        catch (Exception ex)
        {
            downloadLink = fallbackUrl;
            Console.WriteLine($"Error: {ex.Message}");
        }

        return downloadLink;
    }

    public async Task<Stream> DownloadFileAsync(string fileUrl)
    {
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
        using HttpResponseMessage response = await client.GetAsync(fileUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Failed to download file");
        }

        Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();
        MemoryStream memoryStream = new MemoryStream();
        await streamToReadFrom.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }

    private async Task StoreFileAsync(string fileUrl)
    {
        Console.WriteLine("Downloading file...");
        Stream streamToReadFrom = await DownloadFileAsync(fileUrl);
        Console.WriteLine("File downloaded");
        string datedFilename = $"{filename}_{DateTime.Now:dd_MM_yyyy}";
        // Store the file locally
        await StoreFileLocally(streamToReadFrom, datedFilename);
        // Upload the file to the local database
        try
        {
            await UploadToLocalDbAsync(streamToReadFrom, datedFilename);
            Console.WriteLine("File uploaded to local database");
        }
        catch(Exception ex)
        {
            // log error
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine("Failed to upload file to local database");
        }
        streamToReadFrom.Position = 0;
        // Create the blob in Azure Blob Storage and upload the file
    }

    private static async Task StoreFileLocally(Stream dataStream, string datedFilename)
    {
        if (dataStream.CanSeek)
        {
            dataStream.Position = 0;
        }
        else
        {
            throw new InvalidOperationException("Stream must be seekable.");
        }

        string savePath = Path.Combine(Environment.CurrentDirectory, datedFilename);
        await using Stream streamToWriteTo = File.Open(savePath, FileMode.Create);
        await dataStream.CopyToAsync(streamToWriteTo);
        dataStream.Position = 0;
    }

    private static async Task UploadToLocalDbAsync(Stream fileStream, string datedFilename)
    {
        Console.WriteLine("Uploading file to local database");
        using HttpClient client = new HttpClient();
        if (fileStream.CanSeek)
        {
            fileStream.Position = 0;
        }

        var form = new MultipartFormDataContent();
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
        form.Add(fileContent, "file", datedFilename);
        var response = await client.PostAsync(localDbEndpoint, form);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Failed to upload file to endpoint. Status code: {response.StatusCode}");
        }
        fileStream.Position = 0;
    }
}
