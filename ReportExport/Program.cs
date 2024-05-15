using DiffCalculator.IndexRecordDiffCalculator;
using DiffCalculator.Model;
using DiffCalculator.Positions.Visitor;
using FileLoader.FileParserStrategy;
using FileLoader.Model;using FileLoader.Reader;
using ReportExport.Output;
using WebScraping;

namespace ReportExport;
using EmailSender;
public class Program
{
    static void Main()
    {   
        using var reader = new FileReader(filePath);

        var parser = new ParserMiddleware();
        parser.SetNewParserStrategy();

        var file1 = parser.ParseFileToList(reader);
        
        using var reader2 = new FileReader(filePath);

        var parser2 = new ParserMiddleware();
        parser.SetNewParserStrategy();

        var file2 = parser.ParseFileToList(reader2);
        
        var diffCalc = new IndexRecordListDiffCalculator(file1, file2);
        var diffs = diffCalc.GetIndexRecordListDiff();


        EmailSender sender = new EmailSender();
        sender.SendEmail();
        
    }
}
