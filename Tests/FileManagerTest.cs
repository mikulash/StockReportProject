using System.Text;

namespace Tests;

public class FileManagerTest
{
    [Test]
    [TestCase("new.csv", true)]
    [TestCase("/aiosrufioajsdfkajsdkfji4o3ijkad/new.csv", false)]
    public async Task FileMangerTest_FilePath(string filePath, bool expectedResponse)
    {
        // Arrange
        var fileManager = new FileManager.FileManager();
        string data = "This is a file simulation.";
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        
        
        using (MemoryStream stream = new MemoryStream(bytes))
        {
            // Act
            var result = await fileManager.SaveStreamToFile(stream, filePath);

            // Assert
            Assert.AreEqual(result.success, expectedResponse);
        }
    }
}