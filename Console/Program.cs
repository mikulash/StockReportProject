// See https://aka.ms/new-console-template for more information

using FileLoader.FileParserStrategy;
using FileLoader.Reader;
using DiffCalculator.IndexRecordDiffCalculator;

// TODO: write the actual paths
var fileReaderA = new FileReader("/path/to/fileA.csv");
var fileReaderB = new FileReader("/path/to/fileB.csv");
var parser = new ParserMiddleware();
        
var recordListA = parser.ParseFileToList(fileReaderA);
var recordListB = parser.ParseFileToList(fileReaderB);

var diffCalc = new IndexRecordListDiffCalculator();
var diffList = diffCalc.GetIndexRecordListDiff(recordListA, recordListB);

foreach (var item in diffList) {
    Console.WriteLine(item.Company + " " + item.SharesDiff);
}