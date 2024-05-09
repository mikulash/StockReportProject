using MailAPI.DTOs.MailSubscriberDTOs.Create;
using MailAPI.DTOs.MailSubscriberDTOs.Filter;
using MailAPI.DTOs.MailSubscriberDTOs.Update;
using Microsoft.AspNetCore.Mvc;

using SubscriberFacade = GenericBusinessLayer.Facades.IGenericFacade<
    MailDataAccessLayer.Models.MailSubscriber, 
    long, 
    GenericBusinessLayer.Services.IGenericService<MailDataAccessLayer.Models.MailSubscriber, long>, 
    MailAPI.DTOs.MailSubscriberDTOs.Create.CreateMailSubscriberDto, 
    MailAPI.DTOs.MailSubscriberDTOs.Update.UpdateMailSubscriberDto, 
    MailAPI.DTOs.MailSubscriberDTOs.View.ViewMailSubscriberDto, 
    MailAPI.DTOs.MailSubscriberDTOs.View.ViewMailSubscriberDto, 
    MailInfrastructure.EntityFilters.MailSubscriberFilter>;

namespace MailWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MailSubscriberController : ControllerBase
{
    private readonly SubscriberFacade _subscriberFacade;

    public MailSubscriberController(SubscriberFacade subscriberFacade)
    {
        _subscriberFacade = subscriberFacade;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateMailSubscriber(CreateMailSubscriberDto createMailSubscriberDto)
    {
        var subscriber = await _subscriberFacade.CreateAsync(createMailSubscriberDto);
        return Created(
            new Uri($"{Request.Path}/{subscriber.Id}", UriKind.Relative),
            subscriber
        );
    }

    [HttpGet]
    public async Task<IActionResult> FetchAll() => Ok(await _subscriberFacade.FetchAllAsync());
    
    [HttpGet]
    [Route("filter")]
    public async Task<IActionResult> FetchAllFiltered([FromQuery] MailSubscriberFilterDto filter) 
        => Ok(await _subscriberFacade.FetchAllFilteredAsync(filter));

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateMailSubscriber(long id, UpdateMailSubscriberDto updateMailSubscriberDto) 
        => Ok(await _subscriberFacade.UpdateAsync(id, updateMailSubscriberDto));

    [HttpGet]
    [Route("all/{id}")]
    public async Task<IActionResult> FindById(long id) => Ok(await _subscriberFacade.FindByIdAsync(id));

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteById(long id)
    {
        await _subscriberFacade.DeleteByIdAsync(id);
        return NoContent();
    }
}
