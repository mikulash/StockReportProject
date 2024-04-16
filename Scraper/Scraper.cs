using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Scraper;

public class Scraper
{
    private readonly ILogger _logger;

    public Scraper(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Scraper>();
    }

    [Function("Scraper")]
    public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {Now}", DateTime.Now);

        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {ScheduleStatusNext}", myTimer.ScheduleStatus.Next);
        }

    }
}
