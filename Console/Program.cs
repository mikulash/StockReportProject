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
List<IVisitor> visitors = new List<IVisitor>()
    { new NewPositionsVisitor(), new IncreasedPositionsVisitor(), new DecreasedPositionsVisitor() };
visitors.ForEach(visitor => visitor.Visit(recordDiffs));