namespace ReportExport.Output;

public class ExportReportContext
{
    private IOutputStrategy _outputStrategy;

    public ExportReportContext(IOutputStrategy outputStrategy)
    {
        _outputStrategy = outputStrategy;
    }

    public void SetOutputStrategy(IOutputStrategy outputStrategy)
    {
        _outputStrategy = outputStrategy;
    }

    public void PrintReport(List<string> positionGroups)
    {
        _outputStrategy.ExportReport(positionGroups);
    }
}
