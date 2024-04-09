// See https://aka.ms/new-console-template for more information

using FileLoader.FileParserStrategy;
using FileLoader.Reader;
using DiffCalculator.IndexRecordDiffCalculator;
using DiffCalculator.Model;
using DiffCalculator.Positions.Visitor;

// TODO: write the actual paths
var fileReaderA = new FileReader("/path/to/fileA.csv");
var fileReaderB = new FileReader("/path/to/fileB.csv");
var parser = new ParserMiddleware();
        
var recordListA = parser.ParseFileToList(fileReaderA);
var recordListB = parser.ParseFileToList(fileReaderB);

var diffCalc = new IndexRecordListDiffCalculator(recordListA, recordListB);
var recordDiffs = diffCalc.GetIndexRecordListDiff();
var newPositionsVisitor = new NewPositionsVisitor();
var positions = new Positions
{
    NewPositions = recordDiffs.Accept(newPositionsVisitor),
    IncreasedPositions = new List<IndexRecordDiffDto>(),
    DecreasedPositions = new List<IndexRecordDiffDto>()
};
foreach (var item in positions.NewPositions) {
    Console.WriteLine(item.Company + " " + item.IsNew);
}