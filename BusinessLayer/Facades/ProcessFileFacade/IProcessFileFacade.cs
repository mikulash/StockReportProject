namespace BusinessLayer.Facades.ProcessFileFacade;

public interface IProcessFileFacade
{
    Task ProcessAndSaveFileAsync(Stream file, string contentType);
}
