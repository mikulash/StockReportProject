using Common;

namespace Console;

public class ArgumentCheck
{
    public ArgumentCheckModel Setup(string[] args)
    {
        var argumentCheck = InitBaseArgValues();
        foreach (var arg in args)
        {
            if (arg == "--once")
            {
                argumentCheck.DownloadFrequency = Frequency.Once;
            }
            else if (arg.StartsWith("--filePath="))
            {
                argumentCheck.FilePathName = arg.Substring("--filePath=".Length);
            }
            else if (arg.StartsWith("--webFilePath="))
            {
                argumentCheck.WebFilePath = arg.Substring("--webFilePath=".Length);
            }
        }

        return argumentCheck;
    }

    private ArgumentCheckModel InitBaseArgValues()
    {
        var fileManager = new FileManager.FileManager();
        return new ArgumentCheckModel
        {
            DownloadFrequency = Frequency.Once,
            FilePathName = fileManager.GetStockCsvFileNameForDate(DateTime.Today),
            WebFilePath = "https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv"
        };
    }
}

public class ArgumentCheckModel
{
    public Frequency DownloadFrequency { get; set; }
    public string FilePathName { get; set; }
    public string WebFilePath { get; set; }
}