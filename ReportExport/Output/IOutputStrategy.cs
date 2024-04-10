namespace ReportExport.Output;

public interface IOutputStrategy
{
    void ExportReport(List<string> positionGroups);
}
