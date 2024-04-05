// using Common;
// using WebScraping;
//
// <<<<<<< HEAD
// using FileLoader.FileParserStrategy;
// using FileLoader.Reader;
// using DiffCalculator.IndexRecordDiffCalculator;
// using DiffCalculator.Model;
// using DiffCalculator.Positions.Visitor;
//
// // TODO: write the actual paths
// var fileReaderA = new FileReader("/path/to/fileA.csv");
// var fileReaderB = new FileReader("/path/to/fileB.csv");
// var parser = new ParserMiddleware();
//         
// var recordListA = parser.ParseFileToList(fileReaderA);
// var recordListB = parser.ParseFileToList(fileReaderB);
//
// var diffCalc = new IndexRecordListDiffCalculator(recordListA, recordListB);
// var recordDiffs = diffCalc.GetIndexRecordListDiff();
// List<IVisitor> visitors = new List<IVisitor>()
//     { new NewPositionsVisitor(), new IncreasedPositionsVisitor(), new DecreasedPositionsVisitor() };
// visitors.ForEach(visitor => visitor.Visit(recordDiffs));
// =======
// namespace Console;
// class Program
// {
//     static async Task Main(string[] args)
//     {
//         System.Console.WriteLine("Welcome to Stock Report App!");
//
//         // arg check and setup
//         var argumentCheck = new ArgumentCheck();
//         var arguments = argumentCheck.Setup(args);
//
//         // File download
//         if (arguments.DownloadFrequency == Frequency.Once)
//             await DownloadOnce(arguments.WebFilePath, arguments.FilePathName);
//     }
//
//     private static async Task DownloadOnce(string baseWebFile, string filePath)
//     {
//         var webFile = new WebFile(baseWebFile);
//         var response = await webFile.DownloadAndSaveTo(filePath);
//         System.Console.WriteLine(response.success
//             ? $"File was successfully downloaded and saved to {filePath}"
//             : $"Error: {response.msg}");
//     }
// }
// >>>>>>> 702a648 (STO-15 download csv, store to file, argument check)
