using WebScraping;

namespace Tests;

public class WebFileTests
{
    [Test]
    [TestCase("https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv", "existenceTestFile.csv", true)]
    [TestCase("2kalkdsj4oika", "existenceTestFile.csv", false)]
    public async Task WebFileTest_FileExistenceAndNotEmpty(string webFilePath, string filePath, bool expectedResponse)
    {
        // Arrange
        var webFile = new WebFile();
        var response = await webFile.DownloadAndSaveTo(webFilePath, filePath);
        
        // Act
        if (response.success)
        {
            if (File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);

                // Assert
                Assert.Greater(fileInfo.Length, 0);
            }
        }
        Assert.AreEqual(response.success, expectedResponse);
        
    }
}