using System.Net;

namespace WebScraping;

public class WebFile
{
    public async Task<(bool success, string msg)> DownloadAndSaveTo(string webFileUrl, string filePath)
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# HttpClient");

            using var response = await client.GetAsync(webFileUrl);

            if (!response.IsSuccessStatusCode)
                return (false, $"Problem while accessing web page. Status code: {response.StatusCode}");
            
            var stream = await response.Content.ReadAsStreamAsync();
            
            var fileManager = new FileManager.FileManager();
            return await fileManager.SaveStreamToFile(stream, filePath);
        }
        catch (Exception ex)
        {
            return (false, $"Problem message: {ex.Message}");
        }
    }
}