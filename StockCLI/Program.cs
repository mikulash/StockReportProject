// NOTE: This class serves only for simple usage purposes, it will be reinvented later


using System.Net.Http.Headers;
using DiffCalculator.IndexRecordDiffCalculator;
using DiffCalculator.Model;
using DiffCalculator.Positions.Visitor;
using FileLoader.FileParserStrategy;
using FileLoader.Model;
using FileLoader.Reader;
using ReportExport.Output;
using WebScraping;

System.Console.WriteLine("Welcome to Stock Reporting CLI");
System.Console.WriteLine("Type 'help' for a list of available commands.");

var listPrev = new List<NullableIndexRecordDto>();
var listCurr = new List<NullableIndexRecordDto>();
var diff = new RecordDiffs();

using (var httpClient = new HttpClient())
{
    using (var form = new MultipartFormDataContent())
    {
        string filePath = "C:\\Users\\Jozef\\Desktop\\540468\\4.Sem\\PV260\\TempDB\\ARK_2024_03_08.csv";
        using (var fs = File.OpenRead(filePath))
        {
            using (var streamContent = new StreamContent(fs))
            {
                using (var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync()))
                {
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                    // "file" parameter name should be the same as the server side input parameter name
                    form.Add(fileContent, "file", "rand");
                    HttpResponseMessage response = await httpClient.PostAsync("http://localhost:5177/api/indexrecord/upload?fileType=csv", form);
                    Console.WriteLine(response);
                }
            }
        }
    }
}

return;


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

static List<NullableIndexRecordDto> LoadFile(string filePath)
{
    using var reader = new FileReader(filePath);

    var parser = new ParserMiddleware();
    parser.SetNewParserStrategy(FileType.Csv);

    //IDecoratorFilter filter = new NullDecoratorFilter();

    return parser.ParseFileToList(reader);
}

static RecordDiffs CalculateDiff(List<NullableIndexRecordDto> prev, List<NullableIndexRecordDto> curr)
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

    var consoleOutputStrategy = new ConsoleOutputStrategy();
    var outputContext = new ExportReportContext(consoleOutputStrategy);
    var stockPositionGroupsFormatted = visitors.Select(visitor => visitor.ToString()).ToList();

    outputContext.PrintReport(stockPositionGroupsFormatted);
}
