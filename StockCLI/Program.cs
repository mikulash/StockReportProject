// NOTE: This class serves only for simple usage purposes, it will be reinvented later


using DiffCalculator.IndexRecordDiffCalculator;
using DiffCalculator.Model;
using DiffCalculator.Positions.Visitor;
using FileLoader.FileParserStrategy;
using FileLoader.Model;using FileLoader.Reader;
using WebScraping;

System.Console.WriteLine("Welcome to Stock Reporting CLI");
System.Console.WriteLine("Type 'help' for a list of available commands.");

var listPrev = new List<IndexRecordDto>();
var listCurr = new List<IndexRecordDto>();
var diff = new RecordDiffs();


while (true)
{
    System.Console.Write("> ");
    string input = System.Console.ReadLine();
    await ExecuteCommand(input);

    if (input.ToLower() == "exit")
        break;
}
return;

async Task ExecuteCommand(string command)
{
    string[] args = command.Split(' ');
    string cmd = args[0].ToLower();

    switch (cmd)
    {
        case "help":
            PrintHelp();
            break;
        case "download":
            await Download(
                args.Length == 1 ? Path.Combine(Environment.CurrentDirectory, "new.csv") : args[1]);
            break;
        case "load_prev":
            listPrev = LoadFile(args[1]);
            break;
        case "load_current":
            listCurr = LoadFile(args[1]);
            break;
        case "calculate_diff":
            diff = CalculateDiff(listPrev, listCurr);
            break;
        case "extract_and_print":
            ExtractAndPrint(diff);
            break;
        case "exit":
            System.Console.WriteLine("Exiting Stock CLI Shell :)");
            break;
        default:
            System.Console.WriteLine($"Command '{cmd}' not found. Type 'help' for available commands.");
            break;
    }
}

static void PrintHelp()
{
    System.Console.WriteLine("Available commands:");
    System.Console.WriteLine("  help\t\tDisplay available commands.");
    System.Console.WriteLine("  download\t<filePathToCsv>");
    System.Console.WriteLine("  load_prev\t<filePathToCsv>");
    System.Console.WriteLine("  load_current\t<filePathToCsv>");
    System.Console.WriteLine("  calculate_diff");
    System.Console.WriteLine("  extract_and_print");
    System.Console.WriteLine("  exit\t\tExit the program.");
}

static async Task Download(string filePath)
{
    var webFileUrl = "https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";
    var webFile = new WebFile();
    var response = await webFile.DownloadAndSaveTo(webFileUrl, filePath);
    System.Console.WriteLine(response.success
        ? $"File was successfully downloaded and saved to {filePath}"
        : $"Error: {response.msg}");
}

static List<IndexRecordDto> LoadFile(string filePath)
{
    using var reader = new FileReader(filePath);

    var parser = new ParserMiddleware();
    parser.SetNewParserStrategy(FileType.Csv);

    return parser.ParseFileToList(reader);
}

static RecordDiffs CalculateDiff(List<IndexRecordDto> prev, List<IndexRecordDto> curr)
{
    var diffCalc = new IndexRecordListDiffCalculator(prev, curr);
    return diffCalc.GetIndexRecordListDiff();
}

static void ExtractAndPrint(RecordDiffs diffs)
{
    var visitors = new List<IVisitor>
    {
        new NewPositionsVisitor(), 
        new IncreasedPositionsVisitor(), 
        new DecreasedPositionsVisitor()
    };
    
    visitors.ForEach(visitor => visitor.Visit(diffs));
    
    // TODO here call the prints
}