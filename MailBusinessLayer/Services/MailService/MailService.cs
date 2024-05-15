using System.Text;
using System.Xml.Linq;
using Castle.Core.Configuration;
using DiffCalculator.Model;
using DiffCalculator.Positions.Visitor;
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
    public async Task<String> SendEmail()
    {
        var url = _configuration.GetSection("StockWebApiURL").Value;
       HttpResponseMessage response = await _client.GetAsync(url + "indexrecorddiff/raw?fundName=ARK&date=2024-03-8");
       RecordDiffs diffs = null;
       if (response.IsSuccessStatusCode)
       {
           diffs = await response.Content.ReadAsAsync<RecordDiffs>();
       }
       
       Console.WriteLine(diffs);
       //_emailSender.Send(GetStringContent(diffs));
       _emailSender.Send(GetHTMLContent(diffs).ToString());

       return await response.Content.ReadAsStringAsync();
    }

    private String GetStringContent(RecordDiffs diffs)
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
        return sb.ToString();
    }

    private XElement GetHTMLContent(RecordDiffs diffs)
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
        return mainContainer;
    }
    
     
}