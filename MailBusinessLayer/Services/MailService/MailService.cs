using System.Text;
using System.Xml.Linq;
using DiffCalculator.Model;
using DiffCalculator.Positions.Visitor;
using MailDataAccessLayer.Enums;
using MailDataAccessLayer.Models;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace MailBusinessLayer.Services.MailService;
public class MailService: IMailService
{   
    private static HttpClient _client = new ();
    private static EmailSender.EmailSender _emailSender;
    private IConfiguration _configuration;
    public MailService(IConfiguration config)
    {
        _configuration = config;
        _emailSender = new EmailSender.EmailSender(_configuration);
    }
    public async Task SendEmail(IEnumerable<SubscriberPreference> subscribersPreferences, DateOnly date)
    { 
        var url = _configuration.GetSection("StockWebApiURL").Value;
        foreach (var subscriberPreference in subscribersPreferences)
        {
            HttpResponseMessage response = await _client.GetAsync(url + $"indexrecorddiff/raw?fundName={subscriberPreference.FundName}&date={date.ToString("yyyy-MM-dd")}");
            RecordDiffs diffs = null;
            if (response.IsSuccessStatusCode)
            {
                diffs = await response.Content.ReadAsAsync<RecordDiffs>();
            }
            
            switch (subscriberPreference.OutputType)
            {
                case OutputType.String:
                    _emailSender.Send(GetStringContent(diffs, subscriberPreference.MailSubscriber.Id),subscriberPreference.MailSubscriber.Email,subscriberPreference.OutputType);
                    break;
                default:
                    _emailSender.Send(GetHtmlContent(diffs, subscriberPreference.MailSubscriber.Id).ToString(),subscriberPreference.MailSubscriber.Email, subscriberPreference.OutputType);
                    break;
            }
        }
        
    }

    private String GetStringContent(RecordDiffs diffs, Guid guid)
    {
        StringBuilder sb = new StringBuilder();
        var visitors = new List<IVisitor>
        {
            new NewPositionsVisitor(),
            new IncreasedPositionsVisitor(),
            new DecreasedPositionsVisitor()
        };

        visitors.ForEach(visitor => visitor.Visit(diffs));
        visitors.ForEach(visitor => sb.Append(visitor.ToString()));
        sb.AppendLine();
        sb.Append("Unsubscribe link:");
        sb.Append($"{_configuration.GetSection("MailWebApiURL").Value}/unsubscribe/{guid}");
        return sb.ToString();
    }

    private XElement GetHtmlContent(RecordDiffs diffs, Guid guid)
    {
        var mainContainer = new XElement("div");
        var visitors = new List<IVisitor>
        {
            new NewPositionsVisitor(),
            new IncreasedPositionsVisitor(),
            new DecreasedPositionsVisitor()
        };
        
        visitors.ForEach(visitor => visitor.Visit(diffs));
        visitors.ForEach(visitor => mainContainer.Add(visitor.ToHtml()));
        mainContainer.Add(new XElement("div")
        {
            Value = "Unsubscribe link:"
        });
        mainContainer.Add(new XElement("div")
        {
            Value = $"{_configuration.GetSection("MailWebApiURL").Value}/unsubscribe/{guid}"
        });
        
        return mainContainer;
    }
    
     
}