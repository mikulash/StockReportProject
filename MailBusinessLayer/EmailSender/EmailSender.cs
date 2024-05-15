
using Microsoft.Extensions.Configuration;

namespace MailBusinessLayer.EmailSender;

using MailKit.Net.Smtp;
using MimeKit;

public class EmailSender
{
    private IConfiguration _configuration;
    public EmailSender(IConfiguration config)
    {
        _configuration = config;
    }
    public void Send(String content){
        var message = new MimeMessage ();
        var address = _configuration.GetSection("SenderMail").Value;
        var name = _configuration.GetSection("SenderName").Value;
        var password = _configuration.GetSection("Password").Value;
        var mailApi = _configuration.GetSection("MailWebApiURL").Value;
        var link = "\n" +
                   "\n" +
                   $"Unsubscribe link {mailApi}/unsubscribe/1";
        message.From.Add (new MailboxAddress (name, address));
        message.To.Add (new MailboxAddress ("David Rusnak", "davidkorusnak@gmail.com"));
        message.Subject = "Report results";

        message.Body = new TextPart ("html") {
            Text = content + link
        };

        using (var client = new SmtpClient ()) {
            client.Connect ("smtp.gmail.com", 587, false);
            client.Authenticate(address, password);
            client.Send (message);
            client.Disconnect (true);
        }
    }
}