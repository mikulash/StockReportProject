namespace StockBusinessLayer.Facades.ProcessFileFacade;

public interface IProcessFileFacade
{
    Task ProcessAndSaveFileAsync(Stream file, string contentType);
}
