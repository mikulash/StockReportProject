using Common;
using Console;

namespace Tests;

public class ArgumentsTests
{
    private readonly ArgumentCheck _argumentCheck = new();
    
    [Test]
    [TestCase(new string[] { "--once" }, Frequency.Once, null, "https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv")]
    [TestCase(new string[] { "--filePath=myFilePath.csv" }, Frequency.Once, "myFilePath.csv", "https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv")]
    [TestCase(new string[] { "--webFilePath=myWebFilePath.csv" }, Frequency.Once, null, "myWebFilePath.csv")]
    public void ArgumentCheckSetupTest(string[] args, Frequency expectedFrequency, string? expectedFilePath, string expectedWebFilePath)
    {
        // Arrange
        
        // Act
        var result = _argumentCheck.Setup(args);

        // Assert
        Assert.AreEqual(expectedFrequency, result.DownloadFrequency);
        Assert.AreEqual(expectedWebFilePath, result.WebFilePath);

        if (expectedFilePath == null)
        {
            Assert.IsTrue(result.FilePathName.StartsWith("stock-" + DateTime.Now.ToString("yyyy-MM-dd")));
        }
        else
        {
            Assert.AreEqual(expectedFilePath, result.FilePathName);
        }
    }
}