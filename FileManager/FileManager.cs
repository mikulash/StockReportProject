namespace FileManager;

public class FileManager
{
    // Returns true on successful stream write to specified file
    public async Task<(bool success, string msg)> SaveStreamToFile(Stream stream, string filePath)
    {
        if (!ValidateFilePath(filePath))
            return (false, $"Invalid file path {filePath}");
        
        try
        {
            await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await stream.CopyToAsync(fileStream);

            return (true, String.Empty);
        }
        catch (Exception ex)
        {
            return (false, $"Problem while creating a file. Exception: {ex.Message}");
        }
    }

    public string GetStockCsvFileNameForDate(DateTime dateTime)
    {
        return $"stock-{dateTime:yyyy-MM-dd}.csv";
    }

    // Returns true when path exists
    private bool ValidateFilePath(string filePath)
    {
        return Path.GetDirectoryName(filePath) != null;
    }
}