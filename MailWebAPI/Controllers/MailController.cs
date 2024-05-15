using MailBusinessLayer.Facades.MailFacade;
using Microsoft.AspNetCore.Mvc;

namespace MailWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MailController: ControllerBase
{
    private readonly IMailFacade _mailFacade;

    public MailController(IMailFacade mailFacade)
    {
        _mailFacade = mailFacade;
    }
    [HttpGet]
    [Route("send")]
    public async Task Send([FromQuery] string fundName, [FromQuery] DateOnly date) => await _mailFacade.Send(fundName,date);
}