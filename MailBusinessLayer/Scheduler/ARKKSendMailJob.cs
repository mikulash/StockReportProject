using MailBusinessLayer.Facades.MailFacade;
using Quartz;

namespace MailBusinessLayer.Scheduler;

public class ARKKSendMailJob : IJob
{
    private const string FundName = "ARKK";
    
    private readonly IMailFacade _mailFacade;

    public ARKKSendMailJob(IMailFacade mailFacade)
    {
        _mailFacade = mailFacade;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        await _mailFacade.Send(FundName, today);
    }
}
