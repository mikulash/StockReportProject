using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Scraper;

public class Scraper
{
    private readonly ILogger _logger;
    private const string url = "https://ark-funds.com/download-fund-materials/";
    private const string filename = "FundHoldings.csv";

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
        IWebDriver? driver = null;
        try
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");
            driver = new ChromeDriver(chromeOptions);
            // driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);

            //agree cookies
            driver.FindElement(By.XPath("//*[@id=\"agree_button\"]")).Click();
            driver.FindElement(By.XPath("//*[@id=\"hs-eu-confirmation-button\"]")).Click();
            // select document type
            IWebElement element = driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div[2]/div/div/div/div/div[2]/div[1]/div/ul/li[8]/a"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            element.Click();
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

    private static async Task DownloadFileAsync(string fileUrl)
    {
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
        using HttpResponseMessage response = await client.GetAsync(fileUrl);
        await using Stream streamToReadFrom = await response.Content.ReadAsStreamAsync();
        string savePath = Path.Combine(Environment.CurrentDirectory, filename);
        await using Stream streamToWriteTo = File.Open(savePath, FileMode.Create);
        await streamToReadFrom.CopyToAsync(streamToWriteTo);
    }
}
