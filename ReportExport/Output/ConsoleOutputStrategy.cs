namespace ReportExport.Output;

public class ConsoleOutputStrategy : IOutputStrategy
{
    public void ExportReport(List<string> positionGroups)
    {
        foreach (var positionGroup in positionGroups)
        {
            Console.WriteLine(positionGroup);
        }
    }
}
