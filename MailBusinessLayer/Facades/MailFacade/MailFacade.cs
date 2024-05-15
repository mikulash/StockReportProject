using MailBusinessLayer.Services;

namespace MailBusinessLayer.Facades.MailFacade;

public class MailFacade: IMailFacade
{
    private readonly IMailService _mailService;
    public MailFacade(IMailService service)
    {
        _mailService = service;
    }
    public async Task<String> Send()
    {
        return await _mailService.SendEmail();
    }
}